document.addEventListener("DOMContentLoaded", function () {
    const editModal = document.getElementById("editModal");
    if (!editModal) return;

    editModal.addEventListener("show.bs.modal", function (event) {
        const button = event.relatedTarget;

        const id = button.getAttribute("data-id");
        const title = button.getAttribute("data-title");
        const start = button.getAttribute("data-start");
        const end = button.getAttribute("data-end");

        document.getElementById("edit-id").value = id;
        document.getElementById("edit-title").value = title;
        document.getElementById("edit-start").value = start;
        document.getElementById("edit-end").value = end;
    });

    if (editModal.dataset.show === "true") {
        const bootstrapModal = new bootstrap.Modal(editModal);
        bootstrapModal.show();
    }
});