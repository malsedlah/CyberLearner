function toggleMenu() {
    var nav = document.getElementById("mainNav");
    if (nav.className.indexOf("open") === -1) {
        nav.className = "main-nav open";
    } else {
        nav.className = "main-nav";
    }
}

function confirmDelete() {
    return confirm("Are you sure you want to delete this record? This cannot be undone.");
}

function confirmSubmit() {
    return confirm("Are you sure you want to submit your answers?");
}

function checkAnswerSelected() {
    var options = document.querySelectorAll(".options input[type=radio]");
    for (var i = 0; i < options.length; i++) {
        if (options[i].checked) {
            return true;
        }
    }
    alert("Please select an answer before continuing.");
    return false;
}

function togglePassword(textBoxId, checkBox) {
    var box = document.getElementById(textBoxId);
    if (checkBox.checked) {
        box.type = "text";
    } else {
        box.type = "password";
    }
}

function toggleStartButton(checkBox, buttonId) {
    document.getElementById(buttonId).disabled = !checkBox.checked;
}

function printPage() {
    window.print();
    return false;
}
