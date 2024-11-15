using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Layout.Element;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Nomina_Web.Models.dbModels;
using iText.Kernel.Pdf.Canvas.Draw;

namespace Sistema_Nomina_Web.Controllers
{
    public class NominaController : Controller
    {
        private readonly DB_NominaContext _context;

        public NominaController(DB_NominaContext context)
        {
            _context = context;
        }

        // Acción para descargar el PDF
        public async Task<IActionResult> GenerarPDF(int id)
        {
            var nomina = await _context.Nominas
                .Include(n => n.Trabajador)
                .ThenInclude(t => t.Periodicidad)
                .Include(n => n.Trabajador.TipoJornada)  // Incluir TipoJornada
                .Include(n => n.Trabajador.TipoSalario)  // Incluir TipoSalario
                .Include(n => n.PeriodoNomina)
                .Include(n => n.Incidencia)
                .FirstOrDefaultAsync(n => n.NominaId == id);

            if (nomina == null)
            {
                return NotFound();
            }

            using (var ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Encabezado
                document.Add(new Paragraph("RECIBO DE NÓMINA")
                    .SetFontSize(20)
                    .SetBold()
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                // Información de la empresa
                document.Add(new Paragraph("PV & P INDUSTRIES S DE RL DE CV")
                    .SetFontSize(14)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                // Tabla con información fiscal de la empresa
                var empresaTable = new Table(2)
                    .UseAllAvailableWidth()
                    .SetMarginTop(10);

                AddTableRow(empresaTable, "RFC:", "PAP170117815");
                AddTableRow(empresaTable, "Régimen Fiscal:", "601 General de Ley Personas Morales");
                AddTableRow(empresaTable, "Lugar de Expedición:", "66636");
                AddTableRow(empresaTable, "Régimen Patronal:", "D3829033108");

                document.Add(empresaTable);

                // Línea separadora
                document.Add(new LineSeparator(new SolidLine()));

                // Información del empleado y período
                var infoTable = new Table(2)
                    .UseAllAvailableWidth()
                    .SetMarginTop(20);

                // Datos del empleado expandidos
                AddTableRow(infoTable, "Empleado:", nomina.Trabajador.Nombre);
                AddTableRow(infoTable, "No. de Empleado:", nomina.Trabajador.TrabajadorId.ToString());
                AddTableRow(infoTable, "RFC:", nomina.Trabajador.Rfc);
                AddTableRow(infoTable, "CURP:", nomina.Trabajador.Curp);
                AddTableRow(infoTable, "NSS:", nomina.Trabajador.Nss);
                AddTableRow(infoTable, "Fecha de Inicio:", nomina.Trabajador.FechaInicio.ToString("dd/MM/yyyy"));
                // Asumiendo que TipoJornada y TipoSalario tienen una propiedad Nombre o Descripcion
                AddTableRow(infoTable, "Tipo de Jornada:", nomina.Trabajador.TipoJornada?.Descripcion ?? "No especificado");
                AddTableRow(infoTable, "Tipo de Salario:", nomina.Trabajador.TipoSalario?.Descripcion ?? "No especificado");
                AddTableRow(infoTable, "Periodicidad de Pago:", nomina.Trabajador.Periodicidad.Nombre);
                AddTableRow(infoTable, "Período:",
                    $"{nomina.PeriodoNomina.FechaInicio:dd/MM/yyyy} al {nomina.PeriodoNomina.FechaFin:dd/MM/yyyy}");

                document.Add(infoTable);

                // Percepciones
        document.Add(new Paragraph("\nPERCEPCIONES")
            .SetFontSize(14)
            .SetBold());

        var percepcionesTable = new Table(2)
            .UseAllAvailableWidth();
        
        decimal factorPeriodicidad = ObtenerFactorPeriodicidad(nomina.Trabajador.Periodicidad.PeriodicidadId.ToString());
        decimal salarioSegunPeriodicidad = nomina.Trabajador.SalarioBase * factorPeriodicidad;

        AddTableRow(percepcionesTable, "Salario Base Mensual:",
            $"${nomina.Trabajador.SalarioBase:N2}");
        AddTableRow(percepcionesTable, $"Salario {nomina.Trabajador.Periodicidad.Nombre}:",
            $"${salarioSegunPeriodicidad:N2}");

        AddTableRow(percepcionesTable, "Salario Base:",
            $"${nomina.Trabajador.SalarioBase:N2}");

        if (nomina.ImporteHorasExtra > 0)
        {
            AddTableRow(percepcionesTable, "Horas Extra:",
                $"${nomina.ImporteHorasExtra:N2}");
        }

        AddTableRow(percepcionesTable, "Total Percepciones:",
            $"${nomina.TotalPercepciones:N2}", true);

        document.Add(percepcionesTable);

        // Deducciones
        document.Add(new Paragraph("\nDEDUCCIONES")
            .SetFontSize(14)
            .SetBold());

        var deduccionesTable = new Table(2)
            .UseAllAvailableWidth();

        AddTableRow(deduccionesTable, "ISR:", $"${nomina.Isr:N2}");
        AddTableRow(deduccionesTable, "IMSS:", $"${nomina.Imss:N2}");

        if (nomina.DescuentoFaltas > 0)
        {
            AddTableRow(deduccionesTable, "Descuento por Faltas:",
                $"${nomina.DescuentoFaltas:N2}");
        }

        if (nomina.OtrasDeducciones > 0)
        {
            AddTableRow(deduccionesTable, "Otras Deducciones:",
                $"${nomina.OtrasDeducciones:N2}");
        }

        AddTableRow(deduccionesTable, "Total Deducciones:",
            $"${nomina.TotalDeducciones:N2}", true);

        document.Add(deduccionesTable);

        // Total Neto
        document.Add(new Paragraph("\nNETO A PAGAR")
            .SetFontSize(14)
            .SetBold());

        var netoTable = new Table(2)
            .UseAllAvailableWidth();

        AddTableRow(netoTable, "Total Neto:",
            $"${nomina.SalarioNeto:N2}", true);

        document.Add(netoTable);

        // Información adicional
        document.Add(new Paragraph($"\nFecha de cálculo: {nomina.FechaCalculo:dd/MM/yyyy HH:mm}")
            .SetFontSize(10)
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));

        // Espacio para firmas
        document.Add(new Paragraph("\n\n"));
        var firmasTable = new Table(2)
            .UseAllAvailableWidth();

        firmasTable.AddCell(new Cell().Add(new Paragraph("_______________________"))
            .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
        firmasTable.AddCell(new Cell().Add(new Paragraph("_______________________"))
            .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

        firmasTable.AddCell(new Cell().Add(new Paragraph("Firma del Empleado"))
            .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
        firmasTable.AddCell(new Cell().Add(new Paragraph("Recursos Humanos"))
            .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

        document.Add(firmasTable);

        document.Close();

        return File(ms.ToArray(), "application/pdf",
            $"Nomina_{nomina.Trabajador.Nombre}_{nomina.PeriodoNomina.FechaInicio:yyyyMMdd}.pdf");
    }
}

// El método AddTableRow permanece igual
private void AddTableRow(Table table, string label, string value, bool isBold = false)
{
    var cell1 = new Cell().Add(new Paragraph(label).SetBold())
        .SetBorder(iText.Layout.Borders.Border.NO_BORDER);

    var valueText = new Paragraph(value);
    if (isBold) valueText.SetBold();

    var cell2 = new Cell().Add(valueText)
        .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
        .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);

    table.AddCell(cell1);
    table.AddCell(cell2);
}

        // GET: Nomina
        public async Task<IActionResult> Index()
        {
            var dB_NominaContext = _context.Nominas
                                           .Include(n => n.PeriodoNomina)
                                           .Include(n => n.Trabajador)
                                           .ThenInclude(t => t.Periodicidad)
                                           .Include(n => n.Incidencia); // Asegúrate de incluir incidencias
            return View(await dB_NominaContext.ToListAsync());
        }


        // GET: Nomina/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Nominas == null)
            {
                return NotFound();
            }

            var nomina = await _context.Nominas
                .Include(n => n.PeriodoNomina)
                .Include(n => n.Trabajador)
                .FirstOrDefaultAsync(m => m.NominaId == id);
            if (nomina == null)
            {
                return NotFound();
            }

            return View(nomina);
        }

        // GET: Nomina/Create
        public IActionResult Create()
        {
            ViewData["PeriodoNominaId"] = new SelectList(_context.PeriodoNominas, "PeriodoNominaId", "PeriodoNominaId");
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadors, "TrabajadorId", "Nombre");
            ViewData["SalarioBase"] = new SelectList(_context.Trabajadors, "SalarioBase", "SalarioBase");
            return View();
        }

        // POST: Nomina/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NominaId,TrabajadorId,PeriodoNominaId,ImporteHorasExtra,DescuentoFaltas,Isr,Imss,OtrasDeducciones,TotalPercepciones,TotalDeducciones,SalarioNeto,FechaCalculo")] Nomina nomina)
        {
            if (ModelState.IsValid)
            {
                // Obtener el trabajador incluyendo su periodicidad
                var trabajador = await _context.Trabajadors
                    .Include(t => t.Periodicidad)
                    .FirstOrDefaultAsync(t => t.TrabajadorId == nomina.TrabajadorId);

                // Obtener la incidencia correspondiente
                var incidenciaEncontrada = await _context.Incidencia
                    .FirstOrDefaultAsync(i => i.TrabajadorId == nomina.TrabajadorId &&
                                               i.PeriodoNominaId == nomina.PeriodoNominaId);

                if (trabajador != null)
                {
                    // Obtener el factor de periodicidad
                    decimal factorPeriodicidad = ObtenerFactorPeriodicidad(trabajador.Periodicidad.PeriodicidadId.ToString());

                    // Calcular salario según periodicidad
                    decimal salarioSegunPeriodicidad = trabajador.SalarioBase * factorPeriodicidad;

                    // Calcular salario diario (basado en salario mensual)
                    decimal salarioDiario = trabajador.SalarioBase / 30m;
                    // Calcular salario por hora (día laboral de 8 horas)
                    decimal salarioPorHora = salarioDiario / 8m;

                    if (incidenciaEncontrada != null)
                    {
                        nomina.IncidenciaId = incidenciaEncontrada.IncidenciaId;

                        // Calcular importe de horas extra (doble del valor normal)
                        nomina.ImporteHorasExtra = incidenciaEncontrada.HorasExtra * (salarioPorHora * 2);

                        // Calcular descuento por faltas (día completo)
                        nomina.DescuentoFaltas = incidenciaEncontrada.Faltas * salarioDiario;
                    }

                    // Calcular percepciones totales según periodicidad
                    nomina.TotalPercepciones = salarioSegunPeriodicidad + (nomina.ImporteHorasExtra ?? 0m);

                    // Calcular ISR usando la tabla del SAT
                    decimal isrMensual = CalcularISR(trabajador.SalarioBase);
                    nomina.Isr = isrMensual * factorPeriodicidad;

                    // Calcular IMSS según periodicidad (2.775% del total de percepciones)
                    decimal imssMensual = (nomina.TotalPercepciones ?? 0m) * 0.02775m;
                    nomina.Imss = imssMensual * factorPeriodicidad;

                    // Calcular deducciones totales
                    nomina.TotalDeducciones = (nomina.Isr ?? 0m) +
                                              (nomina.Imss ?? 0m) +
                                              (nomina.DescuentoFaltas ?? 0m) +
                                              (nomina.OtrasDeducciones ?? 0m);

                    // Calcular salario neto
                    nomina.SalarioNeto = (nomina.TotalPercepciones ?? 0m) - (nomina.TotalDeducciones ?? 0m);
                    nomina.FechaCalculo = DateTime.Now;
                }

                _context.Add(nomina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PeriodoNominaId"] = new SelectList(_context.PeriodoNominas, "PeriodoNominaId", "PeriodoNominaId", nomina.PeriodoNominaId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadors, "TrabajadorId", "TrabajadorId", nomina.TrabajadorId);
            return View(nomina);
        }

        // Método para calcular el ISR basado en la tabla del SAT 2024
        private decimal CalcularISR(decimal salarioMensual)
        {
            // Tabla de tarifas del ISR mensual 2024 (ejemplo simplificado)
            var tarifasISR = new[]
            {
        new { LimiteInferior = 0m, LimiteSuperior = 644.58m, CuotaFija = 0m, PorcentajeExcedente = 0.0192m },
        new { LimiteInferior = 644.59m, LimiteSuperior = 5470.92m, CuotaFija = 12.38m, PorcentajeExcedente = 0.0640m },
        new { LimiteInferior = 5470.93m, LimiteSuperior = 9614.66m, CuotaFija = 321.26m, PorcentajeExcedente = 0.1088m },
        new { LimiteInferior = 9614.67m, LimiteSuperior = 11176.62m, CuotaFija = 772.10m, PorcentajeExcedente = 0.16m },
        new { LimiteInferior = 11176.63m, LimiteSuperior = 13381.47m, CuotaFija = 1022.01m, PorcentajeExcedente = 0.1792m },
        new { LimiteInferior = 13381.48m, LimiteSuperior = 26988.50m, CuotaFija = 1417.12m, PorcentajeExcedente = 0.2136m },
        new { LimiteInferior = 26988.51m, LimiteSuperior = 42537.58m, CuotaFija = 4323.58m, PorcentajeExcedente = 0.2352m },
        new { LimiteInferior = 42537.59m, LimiteSuperior = 81211.25m, CuotaFija = 7980.73m, PorcentajeExcedente = 0.30m },
        new { LimiteInferior = 81211.26m, LimiteSuperior = 108281.67m, CuotaFija = 19582.83m, PorcentajeExcedente = 0.32m },
        new { LimiteInferior = 108281.68m, LimiteSuperior = decimal.MaxValue, CuotaFija = 28245.36m, PorcentajeExcedente = 0.35m }
    };

            // Buscar el rango correspondiente para el salario mensual
            var tarifa = tarifasISR.FirstOrDefault(t => salarioMensual >= t.LimiteInferior && salarioMensual <= t.LimiteSuperior);

            if (tarifa != null)
            {
                decimal excedente = salarioMensual - tarifa.LimiteInferior;
                decimal isrCalculado = tarifa.CuotaFija + (excedente * tarifa.PorcentajeExcedente);
                return isrCalculado;
            }

            return 0m;
        }

        private decimal ObtenerFactorPeriodicidad(string periodicidadId)
        {
            return periodicidadId switch
            {
                "1" => 1m / 30m,        // Diario (1/30 del salario mensual)
                "2" => 7m / 30m,        // Semanal (7/30 del salario mensual)
                "3" => 14m / 30m,       // Catorcenal (14/30 del salario mensual)
                "4" => 15m / 30m,       // Quincenal (15/30 del salario mensual)
                "5" => 1m,              // Mensual (salario completo)
                _ => 1m                // Por defecto mensual
            };
        }


        // GET: Nomina/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Nominas == null)
            {
                return NotFound();
            }

            var nomina = await _context.Nominas.FindAsync(id);
            if (nomina == null)
            {
                return NotFound();
            }
            ViewData["PeriodoNominaId"] = new SelectList(_context.PeriodoNominas, "PeriodoNominaId", "PeriodoNominaId", nomina.PeriodoNominaId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadors, "TrabajadorId", "TrabajadorId", nomina.TrabajadorId);
            return View(nomina);
        }

        // POST: Nomina/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NominaId,TrabajadorId,PeriodoNominaId,SalarioBase,HorasExtra,ImporteHorasExtra,DescuentoFaltas,Isr,Imss,OtrasDeducciones,TotalPercepciones,TotalDeducciones,SalarioNeto,FechaCalculo")] Nomina nomina)
        {
            if (id != nomina.NominaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nomina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NominaExists(nomina.NominaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PeriodoNominaId"] = new SelectList(_context.PeriodoNominas, "PeriodoNominaId", "PeriodoNominaId", nomina.PeriodoNominaId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadors, "TrabajadorId", "TrabajadorId", nomina.TrabajadorId);
            return View(nomina);
        }

        // GET: Nomina/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Nominas == null)
            {
                return NotFound();
            }

            var nomina = await _context.Nominas
                .Include(n => n.PeriodoNomina)
                .Include(n => n.Trabajador)
                .FirstOrDefaultAsync(m => m.NominaId == id);
            if (nomina == null)
            {
                return NotFound();
            }

            return View(nomina);
        }

        // POST: Nomina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Nominas == null)
            {
                return Problem("Entity set 'DB_NominaContext.Nominas'  is null.");
            }
            var nomina = await _context.Nominas.FindAsync(id);
            if (nomina != null)
            {
                _context.Nominas.Remove(nomina);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NominaExists(int id)
        {
          return (_context.Nominas?.Any(e => e.NominaId == id)).GetValueOrDefault();
        }
    }
}
