document.addEventListener("DOMContentLoaded", function () {
    const forms = document.querySelectorAll(".add-to-cart-form");

    forms.forEach(form => {
        form.addEventListener("submit", function (e) {
            e.preventDefault();

            const productId = form.getAttribute("data-product-id");
            const productName = form.getAttribute("data-product-name");
            const productPrice = parseFloat(form.getAttribute("data-unit-price"));

            fetch("/Cart/AddJson", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    productId: productId,
                    productName: productName,
                    unitPrice: productPrice,
                    quantity: 1
                }),
            })
            .then(res => res.json())
            .then(data => {
                if (data.success) {
                    const badge = document.getElementById("cart-count");
                    if (badge) {
                        badge.innerText = data.cartCount;
                    }
                } else {
                    alert(data.message || "Failed to add to cart.");
                }
            })
            .catch(err => {
                console.error("Error:", err);
            });
        });
    });
});
