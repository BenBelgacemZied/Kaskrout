# Kaskrout

Petite application .NET MAUI de mini-jeux pour passer le temps, sans règles compliquées.

## Jeux disponibles

- **Attrape les étoiles** : trouve et tape l’étoile dans une grille avant la fin du chrono.
- **Réflexe** : attends le feu vert et appuie le plus vite possible.
- **Lance le dé** : lance le dé autant de fois que tu veux.
- **Pile ou face** : laisse la pièce choisir.
- **Machine surprise** : découvre un emoji au hasard.
- **Éclate les bulles** : tape le plus de bulles en 20 secondes.

Les points gagnés sont sauvegardés sur l’appareil.

## Lancer avec Visual Studio

Ouvre `Kaskrout.csproj` dans Visual Studio avec le workload .NET MAUI, puis choisis un émulateur Android.

## APK Android

Chaque push sur `main` lance le workflow **Build Kaskrout Android APK**. Après le succès, télécharge `Kaskrout-Android-APK` depuis l’onglet **Actions** du dépôt. Tu peux aussi lancer le workflow manuellement depuis Actions.

Pour compiler localement avec .NET 10 :

```sh
dotnet workload install maui-android
dotnet publish Kaskrout.csproj -f net10.0-android -c Release -p:TargetFrameworks=net10.0-android -p:AndroidPackageFormat=apk
```
