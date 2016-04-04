using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour {

    public bool playTuto = true;
    public Text text;
    public CanvasGroup tutorial;
    public Animator farmer;
    public AudioSource stomp;
    public AudioSource bip;

    public void StartTutorial () {

        tutorial.gameObject.SetActive(playTuto);

        if (playTuto)
            StartCoroutine(Tutorial());
	}
	
	IEnumerator Tutorial()
    {
        yield return null;
        GameManager.instance.allowAction = false;
        GameManager.instance.timePass = false;
        yield return ShowTextBlocking("Heû l'jeunôt ! c'est bentôt le temps des corvées, chie'd'poule !");
        yield return ShowTextBlocking("Ca fait 2 jours qu't'es là, t'as toujours pas mis les bottes dans le pré, pedzouille !");
        yield return ShowTextBlocking("Aujourd'hui, J'm'en va t'apprendre le RITUEL d'nous autres les paysans. On cultive des légumes du matin au soir.");
        yield return ShowText("Pour planter ton premier légume d'ta vie, appuie donc sur la barre d'espace.");
        yield return WaitForKeyCode(KeyCode.Space);
        yield return ShowText("C'est pas dommage. t'as pas que de l'air dans le ciboulot on dirait. Maintenant appuie sur [GAUCHE] pour choisir ce que tu vas planter.");
        yield return WaitForPlant();
        yield return ShowText("Prend donc des carottes, ça te rendra aimable. Enfin si t'arrives à les faire pousser jusqu'au bout.");
        yield return WaitForCarrot();
        yield return ShowText("Pour les planter correctement, va falloir que t'appuies sur les bonnes touches, c'pas facile mais t'es un dégourdi je suppose.");
        yield return WaitForCarrotPlanted();
        yield return ShowText("Bien, tes carottes sont en train de pousser, il faut être patient. Bientôt elle auront besoin d'eau.");
        yield return WaitForCarrotNeedsWater();
        yield return ShowText("C'est le moment des les arroser. Je vais quand même pas t'expliquer comment arroser des plantes, hein ?");
        yield return WaitForCarrotWatered();
        yield return ShowText("Bien bien, tu devras les arroser encore une fois avant de pouvoir les récolter. Faut pas traîner sinon elle finiront six pieds sous terre.");
        yield return WaitForCarrotCanBeHarvested();
        yield return ShowText("C'est maintenant, ramasse donc tes carrotes et viens les mettre dans la marmitte, c'est bien le temps de se remplir la panse !");
        yield return WaitForCarrotHarvested();
        yield return ShowTextBlocking("A chaque fois que tu récoltes des légumes, tu gagne des Points LEGUM (PL), c'est comme ça qu'on fait vivre la ferme.");
        yield return ShowTextBlocking("Si tu veux gagner plus de points légumes, tu peux faire des combos en ramassant plusieurs fois le même légume.");
        yield return ShowTextBlocking("Tu peux aussi planter les légumes appropriés sur les terrain correspondant pour gagner encore plus de Points LEGUM.");
        yield return ShowTextBlocking("Encore un un chose, je vais pas t'faire travailler jusqu'au soir alors une fois la journée fini, on arrête et on passe à un autre champ le lendemain !");
        yield return ShowTextBlocking("Au boulot, cul'd'asticot !");

        tutorial.gameObject.SetActive(false);
        GameManager.instance.allowAction = true;
        GameManager.instance.timePass = true;
    }

    IEnumerator ShowTextBlocking(string str, float delay = 0.03f)
    {
        farmer.SetTrigger("stomp");
        StartCoroutine(Coroutines.Play(stomp, 0.25f));
        text.text = "";
        float clock = 0f;
        int i = 0;
        while (i < str.Length)
        {
            clock += Time.deltaTime;
            while (clock > delay)
            {
                i++;
                clock -= delay;
                text.text = str.Substring(0, i);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                i = str.Length;
                text.text = str.Substring(0, i);
            }

            yield return null;
        }
        text.text = str;

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        yield return null;
    }

    IEnumerator ShowText(string str, float delay = 0.03f)
    {
        farmer.SetTrigger("stomp");
        StartCoroutine(Coroutines.Play(stomp, 0.25f));
        text.text = "";
        float clock = 0f;
        int i = 0;
        while (i < str.Length)
        {
            clock += Time.deltaTime;
            while (clock > delay)
            {
                i++;
                clock -= delay;
                text.text = str.Substring(0, i);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                i = str.Length;
                text.text = str.Substring(0, i);
            }

            yield return null;
        }
        text.text = str;
        yield return null;
    }

    IEnumerator WaitForKeyCode(KeyCode key)
    {
        GameManager.instance.allowAction = true;
        while(!Input.GetKeyDown(key)) { yield return null; }
        GameManager.instance.allowAction = false;
    }
    IEnumerator WaitForPlant()
    {
        GameManager.instance.allowAction = true;
        while (GameManager.instance.inputType != GameManager.InputType.CHOOSEPLANT) { yield return null; }
        GameManager.instance.allowAction = false;
    }
    IEnumerator WaitForCarrot()
    {
        GameManager.instance.allowAction = true;
        while (GameManager.instance.inputType != GameManager.InputType.ACTIONPLANTCARROT) { yield return null; }
        GameManager.instance.allowAction = false;
    }
    IEnumerator WaitForCarrotPlanted()
    {
        GameManager.instance.allowAction = true;
        while (GameManager.instance.inputType != GameManager.InputType.MOVING) { yield return null; }
        GameManager.instance.allowAction = false;
    }
    IEnumerator WaitForCarrotNeedsWater()
    {
        GameManager.instance.allowAction = true;
        Tile plantedTile = GameObject.Find("carrot(Clone)").transform.parent.GetComponent<Tile>();
        while (!plantedTile.needsWater) { yield return null; }
        GameManager.instance.allowAction = false;
    }
    IEnumerator WaitForCarrotWatered()
    {
        GameManager.instance.allowAction = true;
        Tile plantedTile = GameObject.Find("carrot(Clone)").transform.parent.GetComponent<Tile>();
        while (plantedTile.needsWater) { yield return null; }
        GameManager.instance.allowAction = false;
    }
    IEnumerator WaitForCarrotCanBeHarvested()
    {
        GameManager.instance.allowAction = true;
        Tile plantedTile = GameObject.Find("carrot(Clone)").transform.parent.GetComponent<Tile>();
        while (!plantedTile.needsWater) { yield return null; }
        while (plantedTile.needsWater) { yield return null; }
        GameManager.instance.allowAction = false;
    }
    IEnumerator WaitForCarrotHarvested()
    {
        GameManager.instance.allowAction = true;
        Tile plantedTile = GameObject.Find("carrot(Clone)").transform.parent.GetComponent<Tile>();
        while (plantedTile.status != Tile.Status.HARVESTED) { yield return null; }
        GameManager.instance.allowAction = false;
    }
}
