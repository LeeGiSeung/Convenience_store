using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class StoreController : MonoBehaviour
{
    public static StoreController instance;
    public float currentMoney = 100000f;
    public Transform stockSpawnPoint, furnitureSpawnPoint;

    public List<FurnitureController> shelvingCases = new List<FurnitureController>();

    public float curStageTime = 0;
    public float stageTime = 10f;

    public List<float> stageTimeList = new List<float>();

    public bool playWave = false;

    public TMP_Text timeValueText;
    public TMP_Text pressEnterText;

    public void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIController.instance.UpdateMoney(currentMoney);

        AudioManager.instance.StartBGM();

        curStageTime = stageTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            AddMoney(100f);
        }
        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            if(CheckMoneyAvailable(250f)){
                SpendMoney(250f);
            }
        }

        curStageTime -= Time.deltaTime;
        timeValueText.text = curStageTime.ToString("F1");

        if(curStageTime <= 0f) //stagetime이 0이하가 되면 스테이지 종료
        {
            SetTimeValueText_False();
        }

    }

    public void AddMoney(float amountToAdd)
    {
        currentMoney += amountToAdd;

        UIController.instance.UpdateMoney(currentMoney);
    }

    public void SpendMoney(float amountToSpend)
    {
        currentMoney -= amountToSpend;

        if(currentMoney < 0)
        {
            currentMoney = 0;
        }

        UIController.instance.UpdateMoney(currentMoney);
    }

    public bool CheckMoneyAvailable(float amountToCheck)
    {
        bool hasEnough = false;

        if(currentMoney >= amountToCheck)
        {
            hasEnough = true;
        }

        return hasEnough;
    }

    public void SetTimeValueText_True()
    {
        Debug.Log("Start Stage");
        pressEnterText.gameObject.SetActive(false);
        timeValueText.gameObject.SetActive(true);
        playWave = true;
        
    }

    public void SetTimeValueText_False()
    {

        Debug.Log("End Stage");
        pressEnterText.gameObject.SetActive(true);
        timeValueText.gameObject.SetActive(false);
        playWave = false;
        curStageTime = stageTime;
    }

}
