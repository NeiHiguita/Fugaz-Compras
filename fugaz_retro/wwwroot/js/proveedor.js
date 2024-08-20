  document.addEventListener('DOMContentLoaded', function () {
             $("[data-toggle='switch']").bootstrapSwitch();
            var tipoProveedorSelect = document.getElementById('tipoProveedorSelect');
      var camposEmpresa = document.getElementById('camposJuridico');
            var camposNatural = document.getElementById('camposNatural');

            tipoProveedorSelect.addEventListener('change', function () {
                if (tipoProveedorSelect.value === 'Juridico') {
                    camposEmpresa.style.display = 'block';
                    camposNatural.style.display = 'none';
                } else if (tipoProveedorSelect.value === 'Natural') {
                    camposEmpresa.style.display = 'none';
                    camposNatural.style.display = 'block';
                } else {
              
                }
            });

            tipoProveedorSelect.dispatchEvent(new Event('change'));
  });
        //Estado por defecto activo
        document.addEventListener('DOMContentLoaded', function () {
            // Obtener el checkbox del estado
            var estadoCheckbox = document.querySelector('[name="Estado"]');

            // Establecer el estado por defecto
            estadoCheckbox.checked = true;
        });

//Boton "Crear" al "Index"
document.addEventListener('DOMContentLoaded', function () {
    var form = document.getElementById('createForm');

    form.addEventListener('submit', function (event) {
        event.preventDefault(); 
        var formData = new FormData(form);
        fetch(form.action, {
            method: form.method,
            body: formData
        })
            .then(function (response) {
                if (response.ok) {
                   
                    window.location.href = '/Proveedors/Index';
                } else {
                    console.error('Error al guardar los datos:', response.statusText);
                }
            })
            .catch(function (error) {
                console.error('Error al guardar los datos:', error);
            });
    });
});
//---------------EDIT-----------------*/
$(document).ready(function () {
    $("#tipoProveedorSelect").change(function () {
        var tipoProveedor = $(this).val();
        if (tipoProveedor === "Juridico") {
            $(".campos-proveedor").hide();
            $("#camposJuridico").show();
        } else if (tipoProveedor === "Natural") {
            $(".campos-proveedor").hide();
            $("#camposNatural").show();
        }
    });
});

/**---------COMPRA----------*/

//CREATE
document.getElementById('subtotal').addEventListener('change', function () {
    // Obtener el valor del subtotal
    var subtotal = parseFloat(this.value);

    // Verificar si el subtotal es un número válido
    if (!isNaN(subtotal)) {
        // Calcular el valor del IVA (19% del subtotal)
        var iva = subtotal * 0.19;

        // Actualizar el campo de IVA con el resultado
        document.getElementById('iva').value = iva.toFixed(2);
    }
});
document.addEventListener('DOMContentLoaded', function () {
    calcularPrecioTotal();

    document.getElementById('subtotal').addEventListener('change', calcularPrecioTotal);
    document.getElementById('iva').addEventListener('change', calcularPrecioTotal);
    document.getElementById('descuento').addEventListener('change', calcularPrecioTotal);
});

function calcularPrecioTotal() {
    var subtotal = parseFloat(document.getElementById('subtotal').value);
    var iva = parseFloat(document.getElementById('iva').value);
    var descuento = parseFloat(document.getElementById('descuento').value);

    if (!isNaN(subtotal) && !isNaN(iva) && !isNaN(descuento)) {
        var precioTotal = subtotal + iva - descuento;
        document.getElementById('precioTotal').value = precioTotal.toFixed(2);
    }
}