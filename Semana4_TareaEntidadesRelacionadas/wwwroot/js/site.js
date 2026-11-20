// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    // 1. Marcar enlace activo en el menú de navegación
    var currentPath = window.location.pathname.toLowerCase();
    $(".navbar-nav .nav-link, .navbar-nav .dropdown-item").each(function () {
        var href = $(this).attr("href");
        if (href && href !== "#" && href !== "/" && currentPath.startsWith(href.toLowerCase())) {
            $(this).addClass("active fw-semibold text-info");
        }
    });

    // 2. Habilitar combos si ya tienen opciones cargadas (por ejemplo, tras un error de validación)
    var $carreraSelect = $("#CarreraId");
    var $cursoSelect = $("#CursoId");

    if ($carreraSelect.length && $carreraSelect.find("option").length > 1) {
        $carreraSelect.prop("disabled", false);
    }

    if ($cursoSelect.length && $cursoSelect.find("option").length > 1) {
        $cursoSelect.prop("disabled", false);
    }

    // 3. Cascada 1: Facultad -> Carreras
    $("#FacultadId").on("change", function () {
        var facultadId = $(this).val();
        var $carrera = $("#CarreraId");
        var $curso = $("#CursoId");

        $carrera
            .empty()
            .append('<option value="">-- Seleccione una Carrera --</option>')
            .prop("disabled", true);

        $curso
            .empty()
            .append('<option value="">-- Seleccione un Curso --</option>')
            .prop("disabled", true);

        if (facultadId) {
            $.ajax({
                url: "/Matriculas/ObtenerCarrerasPorFacultad",
                type: "GET",
                data: { facultadId: facultadId },
                success: function (data) {
                    if (data && data.length > 0) {
                        $.each(data, function (i, item) {
                            $carrera.append(
                                $("<option>", {
                                    value: item.id,
                                    text: item.nombre
                                })
                            );
                        });
                        $carrera.prop("disabled", false);
                    } else {
                        $carrera.append('<option value="">No hay carreras registradas</option>');
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error al obtener carreras:", status, error);
                    alert("Ocurrió un error al cargar las carreras.");
                }
            });
        }
    });

    // 4. Cascada 2: Carrera -> Cursos
    $("#CarreraId").on("change", function () {
        var carreraId = $(this).val();
        var $curso = $("#CursoId");

        $curso
            .empty()
            .append('<option value="">-- Seleccione un Curso --</option>')
            .prop("disabled", true);

        if (carreraId) {
            $.ajax({
                url: "/Matriculas/ObtenerCursosPorCarrera",
                type: "GET",
                data: { carreraId: carreraId },
                success: function (data) {
                    if (data && data.length > 0) {
                        $.each(data, function (i, item) {
                            $curso.append(
                                $("<option>", {
                                    value: item.id,
                                    text: item.nombre
                                })
                            );
                        });
                        $curso.prop("disabled", false);
                    } else {
                        $curso.append('<option value="">No hay cursos registrados</option>');
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error al obtener cursos:", status, error);
                    alert("Ocurrió un error al cargar los cursos.");
                }
            });
        }
    });
});