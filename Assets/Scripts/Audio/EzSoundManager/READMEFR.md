# EzSoundManager

## Introduction

EzSoundManager est un système complet de gestion audio pour Unity, conçu pour faciliter le contrôle facile et efficace
du son et de la musique dans les jeux et les applications interactives. Créé par KZ, cet élément simplifie la mise en
œuvre de fonctionnalités audio courantes telles que la lecture de musique, d'effets sonores, la gestion des pools audio
et les transitions audio.

## Installation

Pour installer et utiliser EzSoundManager dans votre projet Unity, suivez ces étapes :

1. Extrayez l'archive EzSoundManager dans le dossier `Assets/Scripts` de votre projet Unity.

2. Pour créer une base de données audio, faites un clic droit sur le dossier `Resources` nouvellement créé et
   choisissez "Créer" > "Audio/Audio Database". Nommez la base de données "AudioDatabase".

3. Remplissez la base de données audio avec des clips audio et des catégories selon vos besoins. Vous pouvez organiser
   les clips audio en catégories et sous-catégories à l'intérieur de l'élément AudioDatabase.

## Utilisation

### Lecture de musique de fond

Pour lire de la musique de fond, utilisez le code suivant :

```csharp
SoundManager.Instance.PlayMusicByCategory("BackgroundTrack", "Music", true);
```

### Lecture d'un effet sonore

Pour lire un effet sonore, utilisez le code suivant :

```csharp
SoundManager.Instance.PlaySoundEffect("Explosion", "SFX", Vector3.zero, true);
```

## Fonctionnalités

- **Modèle Singleton :** Gestion centralisée de la lecture audio.
- **Mise en pool audio :** Gestion efficace des sources audio.
- **Prise en charge du son 3D :** Lecture de sons avec une conscience spatiale.
- **Musique et effets sonores :** Gestion séparée des pistes musicales et des effets sonores.
- **Contrôle dynamique du volume :** Ajustez les volumes pour différents groupes audio.
- **Fondu et fondu enchaîné :** Transitions fluides pour les pistes audio.
- **Sélection aléatoire de hauteur de ton et de son :** Variation dans la lecture audio.

## Configuration

Vous pouvez configurer EzSoundManager en ajustant les paramètres suivants dans l'inspecteur :

- Paramètres du mixeur audio
- Taille du pool pour les sources audio
- Niveaux de volume pour différentes catégories audio

## Contributeurs

- KZ (Créateur)

---

## EzSoundManager AudioPlayer

La classe `AudioPlayer` est une partie cruciale de l'élément EzSoundManager, responsable de la gestion des sources audio
et de la lecture des effets sonores dans votre jeu. Voici un guide complet sur la manière d'utiliser efficacement la
classe `AudioPlayer` :

### Ajout de l'AudioPlayer

1. Attachez le composant `AudioPlayer` à un GameObject de votre scène. Vous pouvez le faire en faisant glisser et en
   déposant le script `AudioPlayer` sur un GameObject dans l'éditeur Unity.

### Configuration du pool audio

2. Dans l'inspecteur, vous pouvez configurer la taille du pool audio en définissant la valeur de `poolSize`. Cela
   détermine combien de sources audio seront pré-créées pour la lecture des effets sonores. Par défaut, le pool contient
   3 sources audio.

### Lecture d'un effet sonore

3. Pour lire un effet sonore, utilisez la méthode `PlaySound` de la classe `AudioPlayer`. Voici un exemple :

```csharp
string nomClip = "Explosion"; // Nom de l'effet sonore
string catégorie = "SFX"; // Catégorie de l'effet sonore
float volume = 1.0f; // Volume de l'effet sonore
bool est3D = true; // L'effet sonore est-il en 3D ou non

AudioPlayer audioPlayer = GetComponent<AudioPlayer>();
audioPlayer.PlaySound(nomClip, catégorie, volume, est3D);
```

- `nomClip` : Le nom de l'effet sonore que vous souhaitez jouer.
- `catégorie` : La catégorie à laquelle appartient l'effet sonore.
- `volume` : Le volume de l'effet sonore (de 0 à 1).
- `est3D` : Détermine si l'effet sonore doit être spatialisé dans l'espace 3D.

### Lecture d'un effet sonore avec fondu

4. L'`AudioPlayer` vous permet également de lire un effet sonore avec un fondu en utilisant la
   méthode `PlaySoundWithFade`. Voici un exemple :

```csharp
string nomClip = "Explosion"; // Nom de l'effet sonore
string catégorie = "SFX"; // Catégorie de l'effet sonore
float duréeFondu = 2.0f; // Durée du fondu en secondes
bool est3D = true; // L'effet sonore est-il en 3D ou non

AudioPlayer audioPlayer = GetComponent<AudioPlayer>();
audioPlayer.PlaySoundWithFade(nomClip, catégorie, duréeFondu, est3D);
```

- `nomClip` : Le nom de l'effet sonore que vous souhaitez jouer.
- `catégorie` : La catégorie à laquelle appartient l'effet sonore.
- `duréeFondu` : La durée du fondu en secondes.
- `volumeCible` : Le volume cible de l'effet sonore (de 0 à 1).
- `est3D` : Détermine si l'effet sonore doit être spatialisé dans l'espace 3D.

### Modification de la hauteur de ton

5. Vous pouvez également modifier la hauteur de ton d'un effet sonore en utilisant la méthode `SetPitch`. Voici un
   exemple :

```csharp
string nomClip = "Explosion"; // Nom de l'effet sonore
float nouvelleHauteurDeTon = 1.2f; // Nouvelle hauteur de ton de l'effet sonore

AudioPlayer audioPlayer = GetComponent<AudioPlayer>();
audioPlayer.SetPitch(nomClip, nouvelleHauteurDeTon);
```

- `nomClip` : Le nom de l'effet sonore que vous souhaitez modifier.
- `nouvelleHa

uteurDeTon` : La nouvelle hauteur de ton de l'effet sonore (1.0f correspond à la hauteur de ton normale, des valeurs
plus élevées l'augmentent et des valeurs plus basses la diminuent).

### Ajout et suppression d'un effet d'écho

6. L'`AudioPlayer` vous permet d'ajouter et de supprimer des effets d'écho à un effet sonore. Voici comment faire :

#### Ajout d'un effet d'écho

```csharp
string nomClip = "Explosion"; // Nom de l'effet sonore
float retard = 0.2f; // Délai de l'écho en secondes
float tauxDeDécroissance = 0.5f; // Taux de décroissance de l'écho

AudioPlayer audioPlayer = GetComponent<AudioPlayer>();
audioPlayer.AddEchoEffect(nomClip, retard, tauxDeDécroissance);
```

- `nomClip` : Le nom de l'effet sonore auquel vous souhaitez ajouter un écho.
- `retard` : Le délai de l'écho en secondes.
- `tauxDeDécroissance` : Le taux de décroissance de l'écho (de 0 à 1).

#### Suppression d'un effet d'écho

```csharp
string nomClip = "Explosion"; // Nom de l'effet sonore

AudioPlayer audioPlayer = GetComponent<AudioPlayer>();
audioPlayer.RemoveEchoEffect(nomClip);
```

- `nomClip` : Le nom de l'effet sonore duquel vous souhaitez supprimer l'écho.

## Conclusion

L'`AudioPlayer` de l'élément EzSoundManager est un outil puissant pour la gestion des effets sonores dans votre jeu
Unity. Il vous permet de lire des effets sonores, de les modifier en temps réel et d'ajouter des effets d'écho pour
créer une expérience sonore immersive pour vos joueurs. Utilisez-le judicieusement pour améliorer l'ambiance et l'
immersion de votre jeu.