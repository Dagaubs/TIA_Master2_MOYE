Auteurs : 
Bordier JB,
Forget Prune,
Gaubert Alan

Voici MakeYourOwnEscape, notre jeu AR sous Unity dans le cadre des cours de TIA en Master 2

Ce jeu utilise des images reconnu par une caméra : voici le lien des images à télécharger et imprimer : https://docs.google.com/document/d/1WfW36BL1gkob0rOvfQqAuP_63Oq34XKQMWynRowJ6po/edit?usp=sharing

dans le jeu vous incarnerez une princesse qui attend son prince charmant dans une haute tour. Cependant, puisque celui-ci ne vient toujours pas, la princesse décide de sortir toute seule et d'aller retrouver le prince pour lui apprendre les bonnes manières.

Votre but sera de placer les cartes, en commançant par la carte "Start" qui contiendra le point de départ comme son nom l'indique, ainsi que le point d'arrivé. Vous pourrez ensuite placer les autres cartes selon votre entendement afin de vous créer votre propre chemin votre objectif : La taverne où se cache sûrement le Prince !

Quand vous aurez fini de placer les pièces, il vous suffira d'appuyer sur la touche "Play" en bas à droite de l'écran, une boite de dialogue apparaîtra, et après un second clique sur l'écran, votre personnage apparaîtra à la base de la tour de la carte "Start". Pour vous déplacer, il vous suffit d'appuyer sur l'écran pour faire apparaître un joystick. Vous avez aussi deux autres boutons sur la droite de l'écran, celui le plus au dessus, le bouton d'action est pour l'instant inutile ; mais le second permet à votre personnage de sauter (en théorie, plus vous rester appuyer sur la touche, plus vous sauter haut !)

Bon courage !

Remarques :

Le jeu est très loin d'être fini et même d'avoir une version jouable.

Nous avons eu beaucoup de mal à travailler avec Vuforia et Android, le manque de Log et la pénibilité pour tester quand on a aucune caméra à accrocher à son ordinateur ralentisse énormément l'avancée du projet.

Nous regrettons aussi de nous être lancé dans un projet qui, nous l'avons compris trop tard, était trop ambitieux et demandais beaucoup trop de travail pour le si peu de temps que nous avions. Nous avons essayé de faire des compromis, mais finalement, nous avons rencontré beaucoups de problème avec Vuforia que nous n'avons pas eu le temps de régler.

Parmi les problèmes relevés :

La physique du saut est totalement bugué, et manque surtout de réajustement de valeurs. N'hésiter pas à appuyer à répétitions sur la touche si vous voulez espérer passer les obstacles, mais bonne chance dans tous les cas!

Nous voulions ajouté des boutons virtuelles sur les cartes pour monter les pièces d'un niveau, le code est écris et devrait être fonctionnel; mais pour une raison inconnue, nous n'arrivions plus à faire fonctionner les boutons virtuels. Nous avions réussi dans une version précédente à faire fonctionner les boutons, mais nous les avions enlevés car on ne s'en servait plus.

Nous voulions faire en sorte que les cartes "s'accroche" aux autres bloc dans "Start" quand les deux devenaient proches pour faciliter le placement des pièces. Mais ce fut quasiment impossible à débuguer avec seulement les Log android et aucune possibilité de toucher à la scène.
