// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const pass1 = document.getElementById('pass1');
const pass2 = document.getElementById('pass2');
const form = document.getElementById('formDatos');
const errorElement = document.getElementById('error');
const errorParaElement = document.getElementById('messageError');

form.addEventListener('submit', (e) => {
    let messages = [];
    if (pass1.value == '' || pass1.value == null) {
        messages.push('Ingrese una contraseña')
    }
    if (pass1.value != pass2.value) {
        messages.push('Las contraseñas no coinciden')
    }
    if (messages.length > 0) {
        e.preventDefault();
        errorElement.className = 'muestra';
        errorParaElement.innerHTML = messages.join(', ');
    }
})