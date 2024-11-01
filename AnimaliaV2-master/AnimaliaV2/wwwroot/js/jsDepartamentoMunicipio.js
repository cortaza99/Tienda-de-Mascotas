$(function () {
    $('#DepartamentoId').change(function () {
        var selectedDepartamento = $(this).val();
        $('#MunicipioId').empty();
        if (selectedDepartamento) {
            $.getJSON('/Mascotas/ObtenerMunicipios', { deptoId: selectedDepartamento }, function (municipios) {
                $.each(municipios, function (index, municipio) {
                    $('#MunicipioId').append($('<option/>', { value: municipio.idMunicipio, text: municipio.nombreMunicipio }));
                });
                $('#MunicipioIdHidden').val($('#MunicipioId').val());
            });
        }
    });
    $('#MunicipioId').change(function () {
        $('#MunicipioIdHidden').val($(this).val());
    });
});

