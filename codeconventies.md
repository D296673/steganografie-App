
# Code Conventions

## Inhoudsopgave
1. [Bestandsnamen](#bestandsnamen)
2. [Namen van Klassen en Methoden](#namen-van-klassen-en-methoden)
3. [Indeling en Spatiëring](#indeling-en-spatiëring)
4. [Commentaar](#commentaar)
5. [XAML Conventies](#xaml-conventies)


## Bestandsnamen
- Bestandsnamen moeten **PascalCase** gebruiken en overeenkomen met de naam van de klasse. Bijvoorbeeld, `MainPage.xaml` en `MainPage.xaml.cs`.
- Vermijd het gebruik van speciale tekens of spaties in bestandsnamen.

## Namen van Klassen en Methoden
- Gebruik **PascalCase** voor klassen- en methodenamen. Bijvoorbeeld, `VerbergTekstInAfbeelding_Click`.
- Variabelen en methoden in C# moeten beschrijvend zijn. Vermijd afkortingen, tenzij het algemeen geaccepteerde afkortingen zijn (zoals `UI` voor User Interface).
- Methode-namen die een gebeurtenis (event) afhandelen, moeten eindigen op `_Click` of een andere actie die de gebeurtenis beschrijft.

## Indeling en Spatiëring
- Gebruik vier spaties per inspringniveau.
- Zorg ervoor dat de code goed leesbaar is door witruimte te gebruiken tussen verschillende codeblokken.
- Openingen van accolades `{` moeten op dezelfde regel staan als de bijbehorende controle-structuur, bijvoorbeeld:
  ```csharp
  if (condition)
  {
      // code
  }
## Commentaar
- gebruik comentaar als het niet duidelijk is wat de functie doet

## XAML Conventies
- Gebruik camelCase voor x:Name attributen en bindings in XAML, zoals x:Name="textInput".
- zorg voor een duidelijke xaml indeling

## commits 
- zorg ervoor dat alle commits in het engels zijn
- duidelijke en korte titel als je meer wilt beschrijven doe dat bij het beschrijven van de commit

