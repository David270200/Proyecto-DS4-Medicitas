<%@ Page Language="C#" AutoEventWireup="true" Async="true"
    CodeBehind="AgendarCita.aspx.cs"
    Inherits="MediCitasWeb.AgendarCita" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <title>Portal - MediCitas</title>

    <style>
  
        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        body {
            min-height: 100vh;
            background: linear-gradient(135deg, #6a11cb, #2575fc);
        }

        .header {
            width: 100%;
            background: rgba(255,255,255,0.9);
            padding: 15px 30px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            box-shadow: 0 4px 10px rgba(0,0,0,0.2);
        }

        .header h1 {
            color: #2575fc;
        }

        /* Buscador */
        .search-bar input {
            padding: 8px 12px;
            border-radius: 8px;
            border: 1px solid #ccc;
            width: 250px;
        }

        /* contenedor de los doctores*/
        .doctores-container {
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            padding: 20px;
            gap: 20px;
        }

        /* diseño para la tarjetas */
        .doctor-card {
            background: #fff;
            border-radius: 15px;
            padding: 20px;
            width: 250px;
            cursor: pointer;
            box-shadow: 0 6px 20px rgba(0,0,0,0.2);
            transition: 0.3s;
        }

        .doctor-card:hover {
            transform: translateY(-5px);
        }

        .doctor-card h3 {
            color: #2575fc;
        }

        .bienvenida {
            text-align: center;
            color: #fff;
            font-size: 22px;
            margin: 20px 0;
            font-weight: bold;
        }

        /* diseño del formulario de citas */
        #formularioCita {
            display: none;
            background: #fff;
            padding: 20px;
            border-radius: 15px;
            width: 300px;
            margin: 20px auto;
            box-shadow: 0 6px 20px rgba(0,0,0,0.2);
        }

        #formularioCita input,
        #formularioCita select {
            width: 100%;
            padding: 8px;
            margin-bottom: 10px;
        }

        #formularioCita button {
            width: 100%;
            padding: 10px;
            background: #2575fc;
            color: #fff;
            border: none;
            border-radius: 8px;
            cursor: pointer;
        }
    </style>
</head>

<body>
  <%
Response.Write("SESSION ID USUARIO = " + (Session["id_usuario"] ?? "NULL"));
%>

<form id="form1" runat="server">

    <!-- BIENVENIDA -->
    <asp:Label ID="lblBienvenida" runat="server" CssClass="bienvenida" />

    <!-- HEADER -->
    <div class="header">
        <h1>MediCitas</h1>
        <div class="search-bar">
            <input type="text"
                   id="txtBuscarEspecialidad"
                   placeholder="Buscar doctores por especialidad..."
                   onkeyup="filtrarDoctores()" />
        </div>
    </div>

    <!-- LISTA DE DOCTORES -->
    <asp:Panel ID="doctoresContainer"
               runat="server"
               CssClass="doctores-container"></asp:Panel>

    <!-- FORMULARIO -->
    <asp:Panel ID="formularioCita" runat="server">
        <h3>Agendar cita con <span id="nombreDoctor"></span></h3>

        <!-- Guarda ID del doctor -->
        <asp:HiddenField ID="hfIdDoctor" runat="server" />

        <!-- Fecha -->
        <input type="date" id="fechaCita" onchange="cargarHorarios()" />

        <!-- Horarios -->
        <select id="horaCita">
            <option value="">Selecciona un horario</option>
        </select>

        <br /><br />
        <button type="button" onclick="agendarCita()">Agendar Cita</button>
        <br /><br />
        <button type="button" onclick="volverADoctores()">Volver</button>
    </asp:Panel>

    <!-- formulario para ver mis citas pendientes -->
    <h3>Mis citas pendientes</h3>

<table border="1" id="tablaCitas">
    <thead>
        <tr>
            <th>Doctor</th>
            <th>Fecha</th>
            <th>Hora</th>
            <th>Estado</th>
        </tr>
    </thead>
    <tbody></tbody>
</table>


    <!-- JAVASCRIPT -->
    <script>
        // BUSCADOR
        function filtrarDoctores() {
            var filtro = document.getElementById("txtBuscarEspecialidad").value.toLowerCase();
            var container = document.getElementById("<%= doctoresContainer.ClientID %>");
            var cards = container.getElementsByClassName("doctor-card");

            for (var i = 0; i < cards.length; i++) {
                cards[i].style.display =
                    cards[i].innerText.toLowerCase().includes(filtro) ? "" : "none";
            }
        }

        // SELECCIONAR DOCTOR
        function seleccionarDoctor(id, nombre) {
            document.getElementById("<%= hfIdDoctor.ClientID %>").value = id;
            document.getElementById("nombreDoctor").innerText = nombre;

            document.getElementById("<%= doctoresContainer.ClientID %>").style.display = "none";
            document.getElementById("<%= formularioCita.ClientID %>").style.display = "block";
        }

        // VOLVER
        function volverADoctores() {
            document.getElementById("<%= formularioCita.ClientID %>").style.display = "none";
            document.getElementById("<%= doctoresContainer.ClientID %>").style.display = "flex";
        }

        // CARGAR HORARIOS 
        function cargarHorarios() {
            var idDoctor = document.getElementById("<%= hfIdDoctor.ClientID %>").value;
            var fechaInput = document.getElementById("fechaCita").value;
            var fecha = fechaInput.split("T")[0]; // asegura yyyy-MM-dd
            var selectHora = document.getElementById("horaCita");

            if (!idDoctor || !fecha) return;

            var fecha = fechaInput.split("T")[0];

            selectHora.innerHTML = "<option>Cargando...</option>";

            var xhr = new XMLHttpRequest();
            var url = "https://localhost:44345/api/citas/disponibles/" +
                idDoctor + "?fecha=" + fecha;

            xhr.open("GET", url, true);

            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        var horarios = JSON.parse(xhr.responseText);
                        selectHora.innerHTML = "<option value=''>Selecciona un horario</option>";

                        for (var i = 0; i < horarios.length; i++) {
                            var opt = document.createElement("option");
                            opt.value = horarios[i];
                            opt.text = horarios[i];
                            selectHora.appendChild(opt);
                        }
                    } else {
                        selectHora.innerHTML = "<option>Error al cargar</option>";
                    }
                }
            };

            xhr.send();
        }
    </script>

    <script>
        var idUsuario = '<%= Session["id_usuario"] ?? "" %>';
    </script>

    <!-- JavaScript para agendar cita -->
    <script>
        function agendarCita() {
            var idDoctor = document.getElementById("<%= hfIdDoctor.ClientID %>").value;
            var fecha = document.getElementById("fechaCita").value;
            var hora = document.getElementById("horaCita").value;

            if (!idDoctor || !fecha || !hora) {
                alert("Completa todos los campos");
                return;
            }

            if (!idUsuario) {
                alert("Sesión expirada. Vuelve a iniciar sesión.");
                return;
            }

            var cita = {
                id_paciente: parseInt(idUsuario),
                id_medico: parseInt(idDoctor),
                fecha: fecha,
                hora: hora
            };

            var xhr = new XMLHttpRequest();
            xhr.open("POST", "https://localhost:44345/api/citas", true);
            xhr.setRequestHeader("Content-Type", "application/json");

            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        alert("Cita agendada correctamente");

                        document.getElementById("<%= formularioCita.ClientID %>").style.display = "none";
                        document.getElementById("<%= doctoresContainer.ClientID %>").style.display = "flex";

                        cargarMisCitas();
                    } else {
                        alert("Error al guardar la cita");
                        console.log(xhr.responseText);
                    }
                }
            };

            xhr.send(JSON.stringify(cita));
        }
    </script>

    <!-- JavaScript para cargar citas del usuario-->
    <script>
        function cargarMisCitas() {
            var xhr = new XMLHttpRequest();
            xhr.open("GET", "https://localhost:44345/api/citas/mis-citas", true);

            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    var citas = JSON.parse(xhr.responseText);
                    var tbody = document.querySelector("#tablaCitas tbody");
                    tbody.innerHTML = "";

                    for (var i = 0; i < citas.length; i++) {
                        var row = `
                    <tr>
                        <td>${citas[i].Doctor}</td>
                        <td>${citas[i].Fecha}</td>
                        <td>${citas[i].Hora}</td>
                        <td>${citas[i].Estado}</td>
                    </tr>`;
                        tbody.innerHTML += row;
                    }
                }
            };

            xhr.send();
        }
    </script>
</form>
</body>
</html>