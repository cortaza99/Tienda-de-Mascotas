$(document).ready(function () {
    // Obtener los datos de la tabla y llenar los modelos correspondientes en el formulario
    $("#formularioRegistro").submit(function (event) {
        event.preventDefault(); // Prevenir el envío del formulario por defecto
       
        //$("#tabla-antecedentes tbody tr").each(function () {
        //    var antecedente = {
        //        antecedente: {
        //            ID: $(this).find("td:nth-child(1)").text(),
        //            Descripcion: $(this).find("td:nth-child(2)").text(),
        //        }
        //    };
        //    registrosAntecedentes.push(antecedente);
            
        //});
        
        //$("#tabla-vacunas tbody tr").each(function () {
        //    var vacunaAnimal = {
        //            ID: $(this).find("td:nth-child(1)").text(),
        //            fkVacuna: $(this).find("td:nth-child(2)").text(),  
        //    };
        //    registroVacunas.push(vacunaAnimal);
        //});

        $("#antecedentesMascota").val(JSON.stringify(registrosAntecedentes)); // Convertir los datos a una cadena JSON y asignarlos al campo oculto
        $("#vacunasMascota").val(JSON.stringify(registroVacunas));
        this.submit(); // Enviar el formulario
    });

});
