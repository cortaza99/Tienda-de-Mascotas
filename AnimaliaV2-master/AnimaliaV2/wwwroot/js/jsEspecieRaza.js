
$(function () {
    $('#EspecieId').change(function () {
        var selectedEspecie = $(this).val();
        $('#RazaId').empty();
        if (selectedEspecie) {
            $.getJSON('/Mascotas/ObtenerRazas', { especieId: selectedEspecie }, function (razas) {
                $.each(razas, function (index, raza) {
                    $('#RazaId').append($('<option/>', { value: raza.idRazaMascota, text: raza.razaMascota }));
                });
                $('#RazaIdHidden').val($('#RazaId').val());
            });
        }
    });
    $('#RazaId').change(function () {
        $('#RazaIdHidden').val($(this).val());
    });
});

