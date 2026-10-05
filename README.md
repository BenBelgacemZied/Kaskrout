# Kaskrout

Application .NET MAUI avec une icône personnalisée et des mini-jeux pour se divertir avec des défis courts, des puzzles et des jeux de hasard.

## Jeux disponibles

- **Puzzle coulissant** : range les tuiles de 1 à 8.
- **Objet manquant** : observe les images et retrouve celle qui a disparu.
- **Jeu des paires** : retourne les cartes et associe les images identiques.
- **Attrape les étoiles** : trouve l’étoile dans la grille avant la fin du chrono.
- **Lance le dé** et **Pile ou face** : mini-jeux de hasard avec animations.
- **Réflexe**, **Éclate les bulles** et **Machine surprise**.

Les points gagnés sont sauvegardés sur l’appareil.

Après l’installation d’une nouvelle version avec une icône modifiée, désinstalle l’ancienne version puis installe le nouvel APK afin de rafraîchir l’icône du lanceur Android.

## Lancer avec Visual Studio

Ouvre `Kaskrout.csproj` dans Visual Studio avec le workload .NET MAUI, puis choisis un émulateur Android.

## APK Android

Chaque mise à jour de `main` lance le workflow **Build Kaskrout Android APK**. Après la réussite, télécharge `Kaskrout-Android-APK` depuis l’onglet **Actions**. L’artefact est conservé 14 jours. Tu peux aussi lancer le workflow manuellement.

Pour compiler localement avec .NET 10 :

```sh
dotnet workload install maui-android
dotnet publish Kaskrout.csproj -f net10.0-android -c Release -p:TargetFrameworks=net10.0-android -p:AndroidPackageFormat=apk
```
