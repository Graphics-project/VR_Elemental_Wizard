using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SkillManager : MonoBehaviour
{
    public InputDevice leftDevice;
    public InputDevice rightDevice;


    public GameObject player;
    public SkillControl fireSkillControl;
    public SkillControl iceSkillControl;
    public SkillControl earthSkillControl;

    private SkillControl currentSkillControl;

    // skills
    public List<GameObject> fireSkills = new List<GameObject>();
    public List<GameObject> iceSkills = new List<GameObject>();
    public List<GameObject> earthSkills = new List<GameObject>();


    public float[] skill_coolTimes = { 1, 6, 10, 15 };
    private float[] skillCooldowns = new float[4];



    // 0: fire
    // 1: ice
    // 2: earth
    public int elementType = 0;


    // private float timeTofire = 0;

    private GameObject skillToSpawn;
    private List<GameObject> currentSkills = new List<GameObject>();
    private bool skillSign = false;

    private int skillNum = 0;


    




    // skill variables (offset)
    private Vector3 simpleProjectile_startPos_offset = new Vector3(0, 0, 2);
    private Quaternion simpleProjectile_rotation_offset = Quaternion.identity;


    // Fire
    private Vector3 fireBreath_startPos_offset = new Vector3(0, 0, 2.5f);
    private Quaternion fireBreath_rotation_offset = Quaternion.identity;

    private Vector3 fireTornado_startPos_offset = new Vector3(0, -0.5f, 10);
    private Quaternion fireTornado_rotation_offset = Quaternion.Euler(0, -90, 0);

    private Vector3 meteor_startPos_offset = new Vector3(0, 25, 0);
    private Quaternion meteor_rotation_offset = Quaternion.Euler(60, 0, 0);



    // Ice
    private Vector3 iceLance_startPos_offset = new Vector3(0, 10, 0);
    private Quaternion iceLance_rotation_offset = Quaternion.Euler(45, 0, 0);

    private Vector3 iceBlizzard_startPos_offset = new Vector3(0, -2, 0);
    private Quaternion iceBlizzard_rotation_offset = Quaternion.identity;

    private Vector3 iceAge_startPos_offset = new Vector3(0, -1.8f, 0);
    private Quaternion iceAge_rotation_offset = Quaternion.identity;

    // Earth
    private Vector3 earthBender_startPos_offset = new Vector3(0, -2f, 7);
    private Quaternion earthBender_rotation_offset = Quaternion.identity;

    private Vector3 earthShatter_startPos_offset = new Vector3(0, -2f, 5);
    private Quaternion earthShatter_rotation_offset = Quaternion.identity;

    private Vector3 golemFoot_startPos_offset = new Vector3(0, -2.5f, 15);
    private Quaternion golemFoot_rotation_offset = Quaternion.identity;



    // Start is called before the first frame update
    void Start()
    {
        InitializeDevices();
        elementType = ElementInit();
        SetElementSkillType(elementType);

        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
    }

    // Update is called once per frame
    void Update()
    {
        if (!leftDevice.isValid || !rightDevice.isValid)
        {
            InitializeDevices();
        }

        skillSelect();
        skillShoot();
        UpdateCooldowns();

        // if (skillSign && Time.time >= timeTofire)
        if (skillSign && skillCooldowns[skillNum] <= 0)
        {
            skillUse();
            skillCooldowns[skillNum] = skill_coolTimes[skillNum];
            currentSkillControl.HideSkillSetting(skillNum);
            currentSkillControl.SetSkillCooldown(skillNum, skill_coolTimes[skillNum]);
        }
        skillSign = false;
    }


    // ===========================================
    //   Functions
    // ===========================================


    void InitializeDevices()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
            {
                leftDevice = device;
            }
            else if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
            {
                rightDevice = device;
            }
        }
    }






    int ElementInit()
    {
        // PlayerPrefs에서 선택된 캐릭터의 인덱스를 불러옴
        int selectedCharacterIndex = PlayerPrefs.GetInt("selectedCharacter", 0); // 기본값으로 0을 사용

        return selectedCharacterIndex;
    }


    // -------------------------------------------
    //  SetElementSkillType()
    // -------------------------------------------

    void SetElementSkillType(int elementType)
    {
        if (elementType == 0)
        {
            currentSkills = fireSkills;
            currentSkillControl = fireSkillControl;
        }
        else if (elementType == 1)
        {
            currentSkills = iceSkills;
            currentSkillControl = iceSkillControl;

        }
        else if (elementType == 2)
        {
            currentSkills = earthSkills;
            currentSkillControl = earthSkillControl;
        }

    }


    // -------------------------------------------
    //  UpdateCooldowns()
    // -------------------------------------------
    void UpdateCooldowns()
    {
        for (int i = 0; i < skillCooldowns.Length; i++)
        {
            if (skillCooldowns[i] > 0)
            {
                skillCooldowns[i] -= Time.deltaTime;
            }
        }
    }




    // -------------------------------------------
    //  skillSelect()
    // -------------------------------------------
    void skillSelect()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        bool left_primaryButtonPressed = false;
        bool left_secondaryButtonPressed = false;
        
        bool right_primaryButtonPressed = false;
        bool right_secondaryButtonPressed = false;

        leftDevice.TryGetFeatureValue(CommonUsages.primaryButton, out left_primaryButtonPressed);
        leftDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out left_secondaryButtonPressed);

        rightDevice.TryGetFeatureValue(CommonUsages.primaryButton, out right_primaryButtonPressed);
        rightDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out right_secondaryButtonPressed);

        if (right_primaryButtonPressed)
        {
            skillNum = 0;
            Debug.Log("right_primary");
        }
        else if (right_secondaryButtonPressed)
        {
            skillNum = 1;
            Debug.Log("right_secondary");
        }
        else if (left_primaryButtonPressed)
        {
            skillNum = 2;
            Debug.Log("left_primary");
        }
        else if (left_secondaryButtonPressed)
        {
            skillNum = 3;
            Debug.Log("left_secondary");
        }
        skillToSpawn = currentSkills[skillNum];
    }


    // -------------------------------------------
    //  skillShoot()
    // -------------------------------------------
    void skillShoot()
    {
        bool left_triggerPressed = false;
        bool right_triggerPressed = false;

        leftDevice.TryGetFeatureValue(CommonUsages.triggerButton, out left_triggerPressed);
        rightDevice.TryGetFeatureValue(CommonUsages.triggerButton, out right_triggerPressed);


        if (left_triggerPressed || right_triggerPressed)
        {
            skillSign = true;
        }
    }



    // -------------------------------------------
    //  skillUse()
    // -------------------------------------------
    void skillUse()
    {

        // Basic Skill(skill 0)
        if (skillNum == 0)
        {
            SpawnSimpleSkills(simpleProjectile_startPos_offset, simpleProjectile_rotation_offset);
        }

        // Epic SKill1(skill 1)
        if (skillNum == 1)
        {
            if (elementType == 0)
            {
                // fire breath
                SpawnSimpleSkills(fireBreath_startPos_offset, fireBreath_rotation_offset);
            }
            else if (elementType == 1)
            {
                // ice lance
                SpawnSimpleSkills(iceLance_startPos_offset, iceLance_rotation_offset);
            }
            else if (elementType == 2)
            {
                // earth bender
                SpawnSimpleSkills(earthBender_startPos_offset, earthBender_rotation_offset);
            }
        }
        // Epic SKill1(skill 2)
        if (skillNum == 2)
        {
            if (elementType == 0)
            {
                // Fire Tornado
                SpawnSimpleSkills(fireTornado_startPos_offset, fireTornado_rotation_offset);
            }
            else if (elementType == 1)
            {
                // ice blizzard
                SpawnSimpleSkills(iceBlizzard_startPos_offset, iceBlizzard_rotation_offset);
            }
            else if (elementType == 2)
            {
                // earth shatter
                SpawnSimpleSkills(earthShatter_startPos_offset, earthShatter_rotation_offset);
            }
        }
        // Ultimate Skill(skill 3)
        if (skillNum == 3)
        {
            if (elementType == 0)
            {
                // Meteor
                SpawnSimpleSkills(meteor_startPos_offset, meteor_rotation_offset);
            }
            else if (elementType == 1)
            {
                // ice age
                SpawnSimpleSkills(iceAge_startPos_offset, iceAge_rotation_offset);
            }
            else if (elementType == 2)
            {
                // golem foot
                SpawnSimpleSkills(golemFoot_startPos_offset, golemFoot_rotation_offset);
            }
        }
    }



    // -------------------------------------------
    //  SpawnSimpleSkills()
    // -------------------------------------------
    void SpawnSimpleSkills(Vector3 startPos_offset, Quaternion startRotate_offest)
    {
        Vector3 startPos = player.transform.TransformPoint(startPos_offset);
        Vector3 forwardDirection = player.transform.forward;
        forwardDirection.y = 0;
        forwardDirection.Normalize(); 

        Quaternion startRotate = Quaternion.LookRotation(forwardDirection) * startRotate_offest;
        Instantiate(skillToSpawn, startPos, startRotate);
    }



}
