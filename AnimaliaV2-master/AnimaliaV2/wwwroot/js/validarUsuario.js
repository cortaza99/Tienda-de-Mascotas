
    $(document).ready(function() {
        // Manejar el evento click del botón Registrar

        $('#butonValidacionUsuario').click(function (e) {
            console.log("Si funciona")
            e.preventDefault(); // Evitar que el formulario se envíe automáticamente

            // Obtener el valor del nombre de usuario ingresado
            var nombreUsuario = $('#nombreUsuario').val();

            // Realizar una solicitud AJAX para verificar si el nombre de usuario existe
            $.ajax({
                url: '/Invitado/VerificarUsuario',
                type: 'POST',
                data: { nombreUsuario: nombreUsuario },
                success: function (result) {
                    if (result.existe) {
                        // Mostrar una advertencia si el nombre de usuario ya existe
                        var mensaje = 'El nombre de usuario ya existe, por favor elija otro';
                        Swal.fire({
                            title: 'Error',
                            text: mensaje,
                            icon: 'error',
                            confirmButtonText: 'Aceptar'
                        });
                    } else {
                        var contra = $('#contra').val();
                        var contraVal= $('#contraVal').val();
                        if (contra != contraVal) {
                            var msj = 'Las contrasenas no coinciden';
                            Swal.fire({
                                title: 'Error',
                                text: msj,
                                icon: 'error',
                                confirmButtonText: 'Aceptar'
                            });
                        } else {
                            // Si el nombre de usuario no existe, enviar el formulario para el registro
                            $('form').unbind('submit').submit();
                        }
                    }
                },
                error: function () {
                    // Manejar errores en la solicitud AJAX si es necesario
                    alert('Ocurrió un error al verificar el nombre de usuario.');
                }
            });
        });
    });

