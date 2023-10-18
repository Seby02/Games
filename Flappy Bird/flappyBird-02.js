// Contexte graphique
const cvs = document.getElementById("zone_de_dessin");
cvs.width = 300;
cvs.height = 400;
const ctx = cvs.getContext("2d");
/** @type {CanvasRenderingContext2D} */ // permet d'utiliser l'extension Canvas et l'autoComplétion

// Déclaration images
const imageArrierePlan = new Image();
imageArrierePlan.src = "images/arrierePlan.png";
const imageAvantPlan = new Image();
imageAvantPlan.src = "images/avantPlan.png";
const imageOiseau1 = new Image();
imageOiseau1.src = "images/oiseau1.png";
const imageOiseau2 = new Image();
imageOiseau2.src = "images/oiseau2.png";
const imageTuyauBas = new Image();
imageTuyauBas.src = "images/tuyauBas.png";
const imageTuyauHaut = new Image();
imageTuyauHaut.src = "images/tuyauHaut.png";

// Paramètres des tuyaux
const largeurTuyau = 40;
const ecartTuyaux = 80;

let tabTuyaux = [];
tabTuyaux[0] = 
{
    x: cvs.width, // largeur 
    y: cvs.height - 150 // hauteur
}
// Paramètres de l'oiseau
let xOiseau = 100;
let yOiseau = 150;
const gravite = 2;
let battements = 0;

document.addEventListener("click", monteOiseau);

function monteOiseau() {
    battements = 10;
    yOiseau = yOiseau - 25;
}

// Dessin
function dessine()
{
    ctx.drawImage(imageArrierePlan, 0, 0);
    // Gestion des tuyaux
    for (let i = 0; i < tabTuyaux.length; i++)
    {
        tabTuyaux[i].x--;
        // Dessin du tuyau    
        ctx.drawImage(imageTuyauBas, tabTuyaux[i].x, tabTuyaux[i].y);
        ctx.drawImage(imageTuyauHaut, tabTuyaux[i].x, tabTuyaux[i].y - ecartTuyaux - imageTuyauHaut.height);
        if(tabTuyaux[i].x === 100) {        
            tabTuyaux.push( // Création d'un tuyau
            {   x: cvs.width,
                y: Math.floor(100 +  Math.random() * 180)
            }) 
        } else if (tabTuyaux[i].x + largeurTuyau < 0) { // on vérifie que le tuyau n'est plus afficher pour pouvoir le supprimer
            tabTuyaux.splice(i, 1); // permet de modifier le contenu du tableau
        }
    }

    ctx.drawImage(imageAvantPlan, 0, cvs.height - imageAvantPlan.height);
    // mouvement de l'oiseau
    yOiseau = yOiseau + gravite;
    if(battements > 0) 
    {
        battements--;
        ctx.drawImage(imageOiseau2, xOiseau, yOiseau);
    } else 
    {
        ctx.drawImage(imageOiseau1, xOiseau, yOiseau);
    }


    ctx.lineWidth = 3;
    ctx.strokeRect(0, 0, cvs.width, cvs.height);
    requestAnimationFrame(dessine); // indique au navigateur qu'on souhaite exécuter une animation et demande que celui-ci exécute une fonction spécifique de mise à jour de l'animation, avant le prochain rafraîchissement à l'écran du navigateur
}
dessine();