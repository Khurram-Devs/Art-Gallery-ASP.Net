

var previewModalImg = document.getElementById("PreviewModal");
previewModalImg.addEventListener('show.bs.modal', function (event) {
    var imageSource = event.relatedTarget.getAttribute('data-bs-image');
    document.getElementById("previewModalImg").src = imageSource
})



let emailEl = document.getElementById("username");
email = emailEl.innerHTML
let atLocation = email.indexOf("@");

let userName = email.slice(0, atLocation);

emailEl.innerHTML = userName


let isSearched = sessionStorage.getItem("search")

if (isSearched) {
    window.location.href = "/#HomeArtsSection"
    setTimeout(() => {
        sessionStorage.removeItem("search")
    }, 1000)
}

let HomeSearchForm = document.getElementById("HomeSearchForm")

HomeSearchForm.addEventListener("submit", () => {
    sessionStorage.setItem("search", true)
})






async function loadCartCount() {
    const usernameEl = document.getElementById("username");
    if (usernameEl) {
        const username = usernameEl.innerText.trim();
        if (username.length === 0) {
            window.location.href = "/Identity/Account/Login";
            return;
        }
    }

    try {
        const response = await fetch(`/Cart/GetTotalItemInCart`);
        if (response.ok) {
            const result = await response.json();
            const cartCountEl = document.getElementById("cartCount");
            if (cartCountEl) {
                cartCountEl.innerHTML = result;

            } else {
                console.error("Element with ID 'cartCount' not found.");
            }
        } else {
            const errorText = await response.text();
            console.error("Fetch error:", response.status, errorText);
        }
    } catch (err) {
        console.error("Fetch error:", err);
    }
}

window.addEventListener("load", loadCartCount())


async function add(artId) {
    const usernameEl = document.getElementById("username");
    if (usernameEl) {
        const username = usernameEl.innerText.trim();
        if (username.length === 0) {
            window.location.href = "/Identity/Account/Login";
            return;
        }
    } else {
        window.location.href = "/Identity/Account/Login";
        return;
    }
    try {
        var response = await fetch(`/Cart/AddItem?artId=${artId}`);
        if (response.ok) {
            var result = await response.json();
            var cartCountEl = document.getElementById("cartCount");
            cartCountEl.innerHTML = result;
            window.location.href = "#cartCount"
        } else {
            var errorText = await response.text();
            console.error("Error:", response.status, errorText);
        }
    } catch (err) {
        console.error("Fetch error:", err);
    }
}

