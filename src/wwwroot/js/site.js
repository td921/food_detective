document.addEventListener("DOMContentLoaded", function ()
{
    const darkModeCheckbox = document.getElementById("darkModeCheckbox");
    const body = document.body;

    darkModeCheckbox.addEventListener("change", function ()
    {
        body.classList.toggle("dark-mode", darkModeCheckbox.checked);
    });
});
