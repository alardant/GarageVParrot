$(document).ready(function () {
    $('.delete').click(function () {
        var reviewId = $(this).data('review-id');
        $('#reviewId').val(reviewId);
    });
});