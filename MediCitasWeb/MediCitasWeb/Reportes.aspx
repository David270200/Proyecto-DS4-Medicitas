<%@ Page Language="C#" AutoEventWireup="true" Async="true"
    CodeBehind="Reportes.aspx.cs" Inherits="MediCitasWeb.Reportes" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <title>Reportes de Citas - MediCitas</title>

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
            padding: 30px;
        }

        .container {
            max-width: 1100px;
            margin: auto;
            background: #ffffff;
            padding: 25px;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.2);
        }

        h2 {
            text-align: center;
            margin-bottom: 25px;
            color: #2575fc;
            font-size: 28px;
        }

        .grid {
            width: 100%;
            border-collapse: collapse;
            margin-top: 15px;
        }

        .grid th {
            background: #2575fc;
            color: #fff;
            padding: 12px;
            text-align: left;
        }

        .grid td {
            padding: 10px;
            border-bottom: 1px solid #ddd;
        }

        .grid tr:nth-child(even) {
            background: #f5f7fb;
        }

        .grid tr:hover {
            background: #e6ecff;
        }

        .mensaje {
            margin-top: 15px;
            text-align: center;
            font-weight: bold;
            color: red;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="container">

            <h2>Reporte de Citas</h2>
            <asp:GridView 
                ID="gvCitas"
                runat="server"
                AutoGenerateColumns="false"
                CssClass="grid">

               <Columns>
                 <asp:BoundField DataField="id_cita" HeaderText="ID Cita" />
                 <asp:BoundField DataField="paciente" HeaderText="Paciente" />
                 <asp:BoundField DataField="medico" HeaderText="Médico" />
                 <asp:BoundField DataField="fecha" HeaderText="Fecha" />
                 <asp:BoundField DataField="hora" HeaderText="Hora" />
                 <asp:BoundField DataField="estado" HeaderText="Estado" />
               </Columns>
     </asp:GridView>

            <asp:Label 
                ID="lblMsg"
                runat="server"
                CssClass="mensaje" />

        </div>
    </form>
</body>
</html>