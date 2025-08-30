document.addEventListener("DOMContentLoaded", function () {
    const editModal = document.getElementById("editModal");
    if (!editModal) return;

    const bootstrapModal = new bootstrap.Modal(editModal);

    editModal.addEventListener("show.bs.modal", function (event) {
        const button = event.relatedTarget;
        if (!button) return;

        document.getElementById("edit-id").value = button.getAttribute("data-id");
        document.getElementById("edit-title").value = button.getAttribute("data-title");
        document.getElementById("edit-start").value = button.getAttribute("data-start");
        document.getElementById("edit-end").value = button.getAttribute("data-end");
    });

    const showId = editModal.dataset.showId;
    if (showId) {
        const button = document.querySelector(`button[data-id='${showId}']`);
        if (button) {
            bootstrapModal.show(); 
            document.getElementById("edit-id").value = button.getAttribute("data-id");
            document.getElementById("edit-title").value = button.getAttribute("data-title");
            document.getElementById("edit-start").value = button.getAttribute("data-start");
            document.getElementById("edit-end").value = button.getAttribute("data-end");
        }
    }
});
