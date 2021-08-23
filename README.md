# AxLabel

Create new labels for Dynamics 365 FinOps without the pain of VS.

PS or CMD the following to create a new label "my new easy label": 

```
label my new easy label
```
And the response will be:

```
@MOD:MyNewEasyLabel
```


Features
--------------------------
- Create a label in multiple files at once (like en-US, en-AU, en-GB)
- Find or create always
- Easy simpe fast, as it should have been.


Configuration
--------------------------
Provide a JSON config (config.JSON) with the following:
```Javascript
{
  "UserCulture": "en-AU", //Your culture, 
  "ModelPrefix": "MOD", //Your model
  "PrimaryFile": "K:\AosService\...\LabelResources\en-AU\MOD.en-AU.label.txt", //Primary file where the lookups will happen
  "LabelFiles":  //List all our files that the label will be created in
  [
    "K:\AosService\...\LabelResources\en-US\MOD.en-US.label.txt",
    "K:\AosService\...\LabelResources\en-AU\MOD.en-AU.label.txt"
  ]
}
```
