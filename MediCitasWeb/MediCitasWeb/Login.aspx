<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MediCitasWeb.Login" Async="true" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <title>Login - MediCitas</title>

    <style>
        * { box-sizing: border-box; margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; }

        body {
            min-height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            background: linear-gradient(135deg, #6a11cb, #2575fc);
            padding: 20px;
        }

        .card {
            background: #fff;
            padding: 40px 30px;
            border-radius: 15px;
            box-shadow: 0 10px 25px rgba(0,0,0,0.2);
            width: 350px;
            text-align: center;
            transition: all 0.4s ease;
        }

        .card h2, .card h3 { margin-bottom: 30px; color: #333; }

        .card label { display: block; text-align: left; margin-bottom: 5px; font-weight: 600; color: #555; }

        .card input[type="text"], .card input[type="password"], .card input[type="email"] {
            width: 100%; padding: 12px 15px; margin-bottom: 20px; border-radius: 8px;
            border: 1px solid #ccc; transition: 0.3s; font-size: 14px;
        }

        .card input:focus { border-color: #2575fc; outline: none; box-shadow: 0 0 5px rgba(37,117,252,0.5); }

        /* botones */
        .btn {
            width: 100%; padding: 14px; border-radius: 50px; font-size: 16px; font-weight: 700; cursor: pointer; transition: all 0.4s ease; margin-bottom: 15px;
        }
        .login-btn { border: none; background: linear-gradient(90deg, #2575fc, #6a11cb); color: #fff; box-shadow: 0 4px 15px rgba(37,117,252,0.4); }
        .login-btn:hover { transform: translateY(-2px); box-shadow: 0 6px 20px rgba(37,117,252,0.6); }

        .register-btn { border: 2px solid #2575fc; background: #fff; color: #2575fc; box-shadow: 0 4px 15px rgba(37,117,252,0.2); }
        .register-btn:hover { background: #2575fc; color: #fff; transform: translateY(-2px); box-shadow: 0 6px 20px rgba(37,117,252,0.4); }

        .submit-register { border: none; background: linear-gradient(90deg, #43e97b, #38f9d7); color: #fff; box-shadow: 0 4px 15px rgba(56,249,215,0.4); }
        .submit-register:hover { transform: translateY(-2px); box-shadow: 0 6px 20px rgba(56,249,215,0.6); }

        .message { margin-top: 15px; color: red; font-weight: 600; font-size: 14px; }
        .footer { margin-top: 20px; font-size: 12px; color: #777; }

        /* ocultar el panel de login */
        .hidden { display: none; }
    </style>

    <script type="text/javascript">
        function mostrarRegistro() {
            document.getElementById('panelLogin').classList.add('hidden');
            document.getElementById('panelRegistro').classList.remove('hidden');
        }

        function mostrarLogin() {
            document.getElementById('panelRegistro').classList.add('hidden');
            document.getElementById('panelLogin').classList.remove('hidden');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <!-- login -->
        <div id="panelLogin" class="card">
            <h2>MediCitas Login</h2>

            <asp:Label Text="Correo:" runat="server" /><br />
            <asp:TextBox ID="txtEmail" runat="server" CssClass="input-field" /><br />

            <asp:Label Text="Contraseña:" runat="server" /><br />
            <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="input-field" /><br />

            <asp:Button ID="btnLogin" runat="server" Text="Ingresar" OnClick="btnLogin_Click" CssClass="btn login-btn" /><br />

            <asp:Button ID="btnGoRegister" runat="server" Text="Registrar" OnClientClick="mostrarRegistro(); return false;" CssClass="btn register-btn" /><br />

            <asp:Label ID="lblMsg" runat="server" CssClass="message" />
            <div class="footer">&copy; 2025 MediCitas. Todos los derechos reservados.</div>
        </div>

        <!-- registro -->
        <div id="panelRegistro" class="card hidden">
            <h3>Registrar Usuario</h3>

            <asp:Label Text="Nombre:" runat="server" /><br />
            <asp:TextBox ID="txtNombre" runat="server" CssClass="input-field" /><br />

            <asp:Label Text="Apellido:" runat="server" /><br />
            <asp:TextBox ID="txtApellido" runat="server" CssClass="input-field" /><br />

            <asp:Label Text="Correo:" runat="server" /><br />
            <asp:TextBox ID="txtEmailReg" runat="server" CssClass="input-field" /><br />

            <asp:Label Text="Contraseña:" runat="server" /><br />
            <asp:TextBox ID="txtPassReg" runat="server" TextMode="Password" CssClass="input-field" /><br />

            <asp:Button ID="btnSubmitRegister" runat="server" Text="Crear Cuenta" OnClick="btnSubmitRegister_Click" CssClass="btn submit-register" /><br />

            <asp:Button ID="btnBackLogin" runat="server" Text="Volver al Login" OnClientClick="mostrarLogin(); return false;" CssClass="btn register-btn" /><br />

            <div class="footer">&copy; 2025 MediCitas. Todos los derechos reservados.</div>
        </div>

    </form>
</body>
</html>