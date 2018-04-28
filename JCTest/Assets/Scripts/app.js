(function ($, document, window) {
    $(function () {
        $('#search-clear').on('click', function (e) {
            e.preventDefault();
            $('#searchInput').val('');

            $(this).closest('form').submit();
        });
    });
})(jQuery, document, window);
