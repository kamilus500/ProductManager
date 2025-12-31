$(document).ready(function () {
    $(".openPopup").on('click', function () {
        const $btn = $(this); 
        const mode = $btn.data('modalmode');
        const productId = $btn.data('id');

        const urls = {
            'create': '/Home/Create',
            'edit': '/Home/Edit/',
            'remove': '/Home/Remove/'
        };

        let finalUrl = urls[mode] || urls['create'];

        if (productId && mode !== 'create') {
            finalUrl += productId;
        }

        $.ajax({
            url: finalUrl,
            type: 'GET',
            success: function (data) {
                $('#modalBody').html(data);
                $('#myModal').modal('show');
            },
            error: function (xhr, status, error) {
                console.error("Error deteils:", error);
            }
        });
    });

    $('#pageSizeSelect').on('change', function () {
        $(this).closest('form').submit();
    });
});