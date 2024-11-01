var registrosAntecedentes = [];
var registroVacunas = [];

let conteoCeldasAntecedentes = 1;

let tablaAntecedentesRegistros = document.getElementById("tabla-antecedentes");


let botonAgregarAntecedente = document.getElementById("btnAgregarAntecedente");

botonAgregarAntecedente.addEventListener("click", function () {

    var nuevoRegistro = document.getElementById("infoAntecedente").value;

    let antecedente = {

        descripcion: nuevoRegistro,
    };

    registrosAntecedentes.push(antecedente);


    var nuevaFila = document.createElement("tr");

    let nuevaCelda1 = document.createElement("td");
    nuevaCelda1.textContent = conteoCeldasAntecedentes;
    conteoCeldasAntecedentes++;

    let nuevaCelda2 = document.createElement("td");
    nuevaCelda2.textContent = nuevoRegistro;

    let nuevaCelda3 = document.createElement("td");
    let fecha = new Date();
    let opciones = { year: 'numeric', month: 'numeric', day: 'numeric' };
    let fechaFormateada = fecha.toLocaleDateString(undefined, opciones);
    nuevaCelda3.textContent = fechaFormateada;


    nuevaFila.appendChild(nuevaCelda1);
    nuevaFila.appendChild(nuevaCelda2);
    nuevaFila.appendChild(nuevaCelda3);


    tablaAntecedentesRegistros.appendChild(nuevaFila);


    document.getElementById("infoAntecedente").value = "";
   
});

let botonEliminarAntecedente = document.getElementById("btnLimpiarAntecedente");

botonEliminarAntecedente.addEventListener("click", function () {

    for (var i = tablaAntecedentesRegistros.rows.length - 1; i > 0; i--) {
        tablaAntecedentesRegistros.deleteRow(i);
    }

    conteoCeldasAntecedentes = 1;
    registrosAntecedentes = [];
});



let conteoCeldasVacunas = 1;


let tablaRegistros = document.getElementById("tabla-vacunas");


let botonAgregarVacuna = document.getElementById("btnAgregarVacuna");

botonAgregarVacuna.addEventListener("click", function () {

    let nuevoRegistro = document.getElementById("infoVacuna").options[document.getElementById("infoVacuna").selectedIndex].text;
    let nuevoRegistroValor = parseInt(document.getElementById("infoVacuna").value);
    let vacunaAnimal = {

        id_vacuna: nuevoRegistroValor,
    };
    registroVacunas.push(vacunaAnimal);

    
    let nuevaFila = document.createElement("tr");

 
    let nuevaCelda1 = document.createElement("td");
    nuevaCelda1.textContent = conteoCeldasVacunas;
    conteoCeldasVacunas++;

    let nuevaCelda2 = document.createElement("td");
    nuevaCelda2.textContent = nuevoRegistro;

    const nuevaCelda3 = document.createElement("td");
    let fecha = new Date();
    let opciones = { year: 'numeric', month: 'numeric', day: 'numeric' };
    let fechaFormateada = fecha.toLocaleDateString(undefined, opciones);
    nuevaCelda3.textContent = fechaFormateada;


    nuevaFila.appendChild(nuevaCelda1);
    nuevaFila.appendChild(nuevaCelda2);
    nuevaFila.appendChild(nuevaCelda3);


    tablaRegistros.appendChild(nuevaFila);

});

let botonEliminarVacuna = document.getElementById("btnLimpiarVacuna");

botonEliminarVacuna.addEventListener("click", function () {

    for (var i = tablaRegistros.rows.length - 1; i > 0; i--) {
        tablaRegistros.deleteRow(i);
    }
    conteoCeldasVacunas = 1;
    registroVacunas = [];
});