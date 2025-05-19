/*
document.addEventListener("DOMContentLoaded", function () {
    const forms = document.querySelectorAll(".add-to-cart-form");

    forms.forEach(form => {
        form.addEventListener("submit", function (e) {
            e.preventDefault();

            const form = e.currentTarget;
            const productId = form.dataset.productId;
            const productName = form.dataset.productName;
            const productPrice = parseFloat(form.dataset.unitPrice);

            console.log("✅ DEBUG JS:", { productId, productName, productPrice });

            fetch("/Cart/AddJson", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    productId,
                    productName,
                    unitPrice: productPrice,
                    quantity: 1
                }),
            })
            .then(res => res.json())
            .then(data => {
                console.log("📦 Full response from server:", data);

                const badge = document.getElementById("cart-count");
                if (data.success) {
                    if (badge && typeof data.cartCount === "number") {
                        badge.textContent = data.cartCount;
                    } else if (badge) {
                        badge.textContent = "0";
                    }

                    showToast("✔️ Added to cart!", "success");
                } else {
                    showToast("❌ Error adding to cart! - NOT in stock", "error");
                }
            })
            .catch(err => {
                console.error("❌ AJAX Error:", err);
            });
        });
    });
});

function showToast(message, type = "success") {
    const color = type === "success" ? "green" : "red";

    const toast = document.createElement("div");
    toast.textContent = message;
    toast.style.position = "fixed";
    toast.style.bottom = "20px";
    toast.style.right = "20px";
    toast.style.backgroundColor = color;
    toast.style.color = "#fff";
    toast.style.padding = "12px 20px";
    toast.style.borderRadius = "6px";
    toast.style.boxShadow = "0 2px 6px rgba(0,0,0,0.2)";
    toast.style.zIndex = "9999";
    toast.style.opacity = "0.95";

    document.body.appendChild(toast);

    setTimeout(() => {
        toast.remove();
    }, 2500);
}
*/