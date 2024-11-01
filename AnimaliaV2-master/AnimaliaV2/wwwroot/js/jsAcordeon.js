const accordionHeaders = document.querySelectorAll(".accordion-header");

accordionHeaders.forEach((accordionHeader) => {
  accordionHeader.addEventListener("click", () => {
    // Cerrar todos los items de la accordion
    const activeAccordionItem = document.querySelector(
      ".accordion-item.active"
    );
    if (activeAccordionItem && activeAccordionItem !== accordionHeader.parentNode) {
      activeAccordionItem.classList.remove("active");
      activeAccordionItem.querySelector(".accordion-content").style.display =
        "none";
    }

    // Abrir o cerrar item de la accordion
    const accordionContent = accordionHeader.nextElementSibling;
    if (accordionHeader.parentNode.classList.contains("active")) {
      accordionHeader.parentNode.classList.remove("active");
      accordionContent.style.display = "none";
    } else {
      accordionHeader.parentNode.classList.add("active");
      accordionContent.style.display = "block";
    }
  });
});