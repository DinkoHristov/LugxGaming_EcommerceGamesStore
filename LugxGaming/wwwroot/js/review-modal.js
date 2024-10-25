// Function to open the modal
function openReviewForm() {
    document.getElementById("reviewModal").style.display = "block";
}

// Function to close the modal
function closeReviewForm() {
    document.getElementById("reviewModal").style.display = "none";
}

// Close the modal when the user clicks outside of it
window.onclick = function (event) {
    if (event.target == document.getElementById("reviewModal")) {
        closeReviewForm();
    }
}

document.querySelectorAll('.star-rating .star').forEach(star => {
    star.addEventListener('click', function () {
        // Remove 'selected' class from all stars
        document.querySelectorAll('.star-rating .star').forEach(s => s.classList.remove('selected'));

        // Add 'selected' class to the clicked star and all previous stars
        this.classList.add('selected');
        let previousSibling = this.previousElementSibling;
        while (previousSibling) {
            previousSibling.classList.add('selected');
            previousSibling = previousSibling.previousElementSibling;
        }

        // Set the hidden input to the selected star's value
        document.getElementById('rating').value = this.getAttribute('data-value');
    });
});

function openAllReviewsModal() {
    document.getElementById("allReviewsModal").style.display = "block";
}

function closeAllReviewsModal() {
    document.getElementById("allReviewsModal").style.display = "none";
}
