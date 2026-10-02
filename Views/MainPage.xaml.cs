using registroProductos.Models;

namespace registroProductos.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public MainPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            CargarProductos(); // Carga la lista al abrir la pantalla
        }

        private async void CargarProductos()
        {
            var productos = await _databaseService.ObtenerProductosAsync();
            listaProductos.ItemsSource = productos;
        }

        private async void OnGuardarProductoClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtDescripcion.Text) ||
                string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                string.IsNullOrWhiteSpace(txtCantidad.Text))
            {
                await DisplayAlert("Error", "Por favor completa todos los campos.", "OK");
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                await DisplayAlert("Error", "Ingresa un precio válido mayor a 0.", "OK");
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad < 0)
            {
                await DisplayAlert("Error", "Ingresa una cantidad válida.", "OK");
                return;
            }

            var nuevoProducto = new Producto
            {
                Nombre = txtNombre.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                Precio = precio,
                Cantidad = cantidad,
                FechaRegistro = DateTime.Now
            };

            await _databaseService.GuardarProductoAsync(nuevoProducto);

            await DisplayAlert("Éxito", "El producto se registró correctamente.", "OK");
            LimpiarFormulario();

            CargarProductos(); // Actualiza la lista al guardar
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            txtCantidad.Text = string.Empty;
        }
    }
}