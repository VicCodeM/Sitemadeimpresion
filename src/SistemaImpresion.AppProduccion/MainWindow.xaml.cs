using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SistemaImpresion.Dominio.DTOs;

namespace SistemaImpresion.AppProduccion;

/// <summary>
/// Ventana principal de la aplicación de producción
/// Permite a los operadores solicitar impresiones al sistema
/// </summary>
public partial class MainWindow : Window
{
    #region Campos Privados

    private readonly HttpClient _httpClient;
    private readonly string _urlAPI;
    private string _identificadorMaquina = string.Empty;

    #endregion

    #region Constructor

    public MainWindow()
    {
        InitializeComponent();

        // Configurar HTTP Client
        _httpClient = new HttpClient();
        _urlAPI = "http://localhost:5023"; // URL de la API

        // Inicializar
        Loaded += MainWindow_Loaded;
    }

    #endregion

    #region Eventos

    /// <summary>
    /// Se ejecuta al cargar la ventana
    /// </summary>
    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await IdentificarMaquinaAsync();
        await VerificarConexionAsync();
    }

    /// <summary>
    /// Maneja el clic en el botón imprimir
    /// </summary>
    private async void BtnImprimir_Click(object sender, RoutedEventArgs e)
    {
        await SolicitarImpresionAsync();
    }

    /// <summary>
    /// Valida que solo se ingresen números
    /// </summary>
    private void SoloNumeros_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !int.TryParse(e.Text, out _);
    }

    #endregion

    #region Métodos Privados

    /// <summary>
    /// Identifica la máquina por hostname
    /// </summary>
    private async Task IdentificarMaquinaAsync()
    {
        await Task.Run(() =>
        {
            try
            {
                _identificadorMaquina = Environment.MachineName;
                Dispatcher.Invoke(() =>
                {
                    txtMaquina.Text = $"Máquina: {_identificadorMaquina}";
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    txtMaquina.Text = "Máquina: Error al detectar";
                    MostrarError($"Error al identificar máquina: {ex.Message}");
                });
            }
        });
    }

    /// <summary>
    /// Verifica la conexión con el servidor
    /// </summary>
    private async Task VerificarConexionAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_urlAPI}/api/impresion/health");
            if (response.IsSuccessStatusCode)
            {
                txtEstadoConexion.Text = "Estado: Conectado al servidor ✓";
                txtEstadoConexion.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                txtEstadoConexion.Text = "Estado: Error de conexión";
                txtEstadoConexion.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        catch
        {
            txtEstadoConexion.Text = "Estado: Sin conexión al servidor";
            txtEstadoConexion.Foreground = new SolidColorBrush(Colors.Red);
        }
    }

    /// <summary>
    /// Solicita una impresión al servidor
    /// </summary>
    private async Task SolicitarImpresionAsync()
    {
        try
        {
            // Validar datos
            if (string.IsNullOrWhiteSpace(txtNumeroEmpleado.Text))
            {
                MostrarError("Debe ingresar el número de empleado");
                txtNumeroEmpleado.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCantidad.Text) || !int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MostrarError("Debe ingresar una cantidad válida");
                txtCantidad.Focus();
                return;
            }

            // Deshabilitar botón
            btnImprimir.IsEnabled = false;
            MostrarInfo("Procesando solicitud...");

            // Crear solicitud
            var solicitud = new SolicitudImpresionDTO
            {
                IdentificadorMaquina = _identificadorMaquina,
                NumeroEmpleado = txtNumeroEmpleado.Text.Trim(),
                Cantidad = cantidad
            };

            // Enviar al servidor
            var response = await _httpClient.PostAsJsonAsync($"{_urlAPI}/api/impresion/solicitar", solicitud);
            var resultado = await response.Content.ReadFromJsonAsync<RespuestaAutorizacionDTO>();

            if (resultado == null)
            {
                MostrarError("Error al procesar la respuesta del servidor");
                return;
            }

            if (resultado.Autorizada && !string.IsNullOrEmpty(resultado.ContenidoZPL))
            {
                // Impresión autorizada - enviar a impresora
                bool exito = await EnviarAImpresoraAsync(resultado.ContenidoZPL, cantidad);

                if (exito)
                {
                    // Confirmar al servidor
                    await ConfirmarImpresionAsync(resultado.ImpresionId ?? 0, true);
                    MostrarExito($"Impresión exitosa: {cantidad} etiqueta(s)");
                    LimpiarFormulario();
                    txtUltimaActividad.Text = $"Última actividad: {DateTime.Now:HH:mm:ss} - {cantidad} etiqueta(s)";
                }
                else
                {
                    await ConfirmarImpresionAsync(resultado.ImpresionId ?? 0, false, "Error al enviar a impresora");
                    MostrarError("Error al enviar a la impresora");
                }
            }
            else
            {
                // Impresión denegada
                MostrarError($"Impresión DENEGADA:\n{resultado.MotivoDenegacion}");
            }
        }
        catch (Exception ex)
        {
            MostrarError($"Error: {ex.Message}");
        }
        finally
        {
            btnImprimir.IsEnabled = true;
        }
    }

    /// <summary>
    /// Envía el código ZPL a la impresora Zebra
    /// </summary>
    private async Task<bool> EnviarAImpresoraAsync(string zpl, int cantidad)
    {
        return await Task.Run(() =>
        {
            try
            {
                // TODO: Implementar envío real a puerto USB
                // Por ahora simular éxito
                System.Threading.Thread.Sleep(500);
                return true;

                /* Código real para enviar a impresora:
                using (var printer = new System.IO.Ports.SerialPort("\\.\USB001"))
                {
                    printer.Open();
                    for (int i = 0; i < cantidad; i++)
                    {
                        printer.Write(zpl);
                    }
                    printer.Close();
                }
                return true;
                */
            }
            catch
            {
                return false;
            }
        });
    }

    /// <summary>
    /// Confirma al servidor el resultado de la impresión
    /// </summary>
    private async Task ConfirmarImpresionAsync(int impresionId, bool exitosa, string? mensajeError = null)
    {
        try
        {
            var confirmacion = new ConfirmacionImpresionDTO
            {
                ImpresionId = impresionId,
                Exitosa = exitosa,
                Resultado = exitosa ? "Exitosa" : "Fallida",
                MensajeError = mensajeError,
                FechaEjecucion = DateTime.UtcNow
            };

            await _httpClient.PostAsJsonAsync($"{_urlAPI}/api/impresion/confirmar", confirmacion);
        }
        catch
        {
            // No propagar error de confirmación
        }
    }

    /// <summary>
    /// Muestra un mensaje de éxito
    /// </summary>
    private void MostrarExito(string mensaje)
    {
        borderMensaje.Visibility = Visibility.Visible;
        borderMensaje.Background = new SolidColorBrush(Color.FromRgb(220, 255, 220));
        borderMensaje.BorderBrush = new SolidColorBrush(Colors.Green);
        txtMensaje.Text = mensaje;
        txtMensaje.Foreground = new SolidColorBrush(Colors.DarkGreen);
    }

    /// <summary>
    /// Muestra un mensaje de error
    /// </summary>
    private void MostrarError(string mensaje)
    {
        borderMensaje.Visibility = Visibility.Visible;
        borderMensaje.Background = new SolidColorBrush(Color.FromRgb(255, 220, 220));
        borderMensaje.BorderBrush = new SolidColorBrush(Colors.Red);
        txtMensaje.Text = mensaje;
        txtMensaje.Foreground = new SolidColorBrush(Colors.DarkRed);
    }

    /// <summary>
    /// Muestra un mensaje informativo
    /// </summary>
    private void MostrarInfo(string mensaje)
    {
        borderMensaje.Visibility = Visibility.Visible;
        borderMensaje.Background = new SolidColorBrush(Color.FromRgb(220, 235, 255));
        borderMensaje.BorderBrush = new SolidColorBrush(Colors.Blue);
        txtMensaje.Text = mensaje;
        txtMensaje.Foreground = new SolidColorBrush(Colors.DarkBlue);
    }

    /// <summary>
    /// Limpia el formulario
    /// </summary>
    private void LimpiarFormulario()
    {
        txtNumeroEmpleado.Clear();
        txtCantidad.Clear();
        txtNumeroEmpleado.Focus();
    }

    #endregion
}