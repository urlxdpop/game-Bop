using TMPro;
using UnityEngine;

[SelectionBase]
public class ComputerBossController : MonoBehaviour, IBoss
{
    [SerializeField] private GameObject _warningBlock;

    private LaserGunController[] _laserGunControllerHor;
    private LaserGunController[] _laserGunControllerVer;
    private LaserGunController[] _laserGuns;

    private int _fazesAttack = 0;
    private int _attackType = 0;
    private int _numAttacks = 0;
    private bool _start = false;

    private float _timer = 0.5f;
    private float _attackTime;
    private bool _attacked;
    private bool _warning;
    private int _stageAttack = 0;
    private int _numLasers;
    private GameObject[] _warningBlocks;
    private bool _isDied = false;

    private const int SIMPLES_ATTACKS = 10;
    private const int HARD_ATTACKS = 10;

    private void Start()
    {
        _laserGunControllerHor = new LaserGunController[12];
        _laserGunControllerVer = new LaserGunController[12];
        _laserGuns = new LaserGunController[24];
        _attackTime = 0;
        _attacked = false;
        _warningBlocks = new GameObject[2000];
    }

    public void Fight()
    {
        if (_isDied) return;

        if (!_start)
        {
            _start = true;
            FindLasers();
        }

        Attack();
    }

    public void SharpAttack() { }

    public int Faze()
    {
        return _fazesAttack;
    }

    private void FindLasers()
    {
        LaserGunController[] laserGunController = FindObjectsByType<LaserGunController>(FindObjectsSortMode.None);

        int x = 0;
        int y = 0;
        foreach (LaserGunController laserGun in laserGunController)
        {
            if (laserGun.transform.rotation == Quaternion.Euler(0, 0, -90))
            {
                _laserGunControllerHor[x++] = laserGun;
            } else
            {
                _laserGunControllerVer[y++] = laserGun;
            }
        }

        _laserGunControllerHor = SortedLaserGuns(_laserGunControllerHor);
        _laserGunControllerVer = SortedLaserGuns(_laserGunControllerVer);
    }

    private void Attack()
    {
        if (_numAttacks >= 12)
        {
            _numAttacks = 0;
            _fazesAttack++;
            if (_fazesAttack == 1)
            {
                _timer = 0.7f;
            } else if (_fazesAttack == 2)
            {
                _timer = 0.5f;
            } else
            {
                Die();
            }
        }

        _attackTime += Time.deltaTime;

        if (_fazesAttack <= 1)
        {
            SimpleAttack();
        } else
        {
            HardAttack();
        }
    }

    private void ActivatedLaser()
    {
        if (_fazesAttack == 0)
        {
            SimpleAttacks();
            Warning();
        } else if (_fazesAttack == 1)
        {
            SimpleAttacks();
            Warning();
        }
    }

    private void SimpleAttack()
    {
        if (!_attacked && !_warning)
        {
            if (_attackTime >= _timer)
            {
                _warning = true;
                _attackTime = 0;
                ActivatedLaser();
            }
        } else if (_warning)
        {
            if (_attackTime >= _timer)
            {
                _attacked = true;
                _warning = false;
                _attackTime = 0;
                SpawnLasers();
            }
        } else
        {
            if (_attackTime >= _timer)
            {
                _attacked = false;
                _attackTime = 0;
                OffLasers();
                _numAttacks++;
            }
        }
    }

    private void SimpleAttacks()
    {
        _attackType = Random.Range(0, SIMPLES_ATTACKS);

        switch (_attackType)
        {
            case 0:
                SetLasersForSimpleAttack(0, 0, 1, 1);
                break;
            case 1:
                SetLasersForSimpleAttack(1, 1, 1, 1);
                break;
            case 2:
                SetLasersForSimpleAttack(0, 0, 2, 2);
                break;
            case 3:
                SetLasersForSimpleAttack(1, 1, 2, 2);
                break;
            case 4:
                SetLasersForSimpleAttack(0, 12, 2, 2);
                break;
            case 5:
                SetLasersForSimpleAttack(12, 1, 2, 2);
                break;
            case 6:
                SetLasersForSimpleAttack(1, 12, 1, 1);
                break;
            case 7:
                SetLasersForSimpleAttack(12, 0, 1, 1);
                break;
            case 8:
                SetLasersForSimpleAttack(0, 0, 3, 1);
                break;
            case 9:
                SetLasersForSimpleAttack(1, 0, 2, 1);
                break;
        }
    }

    private void HardAttack()
    {
        if (_attackTime >= _timer)
        {
            _attackTime = 0;

            if (_stageAttack == 0)
            {
                _attackType = Random.Range(0, HARD_ATTACKS);
            }

            _stageAttack++;

            HardAttacks();
        }
    }

    private void HardAttacks()
    {
        switch (_attackType)
        {
            case 0:
                Attack0(Random.Range(0, 2));
                break;
            case 1:
                Attack1();
                break;
            case 2:
                Attack2();
                break;
            case 3:
                Attack3();
                break;
            case 4:
                Attack4();
                break;
            case 5:
                Attack5();
                break;
            case 6:
                Attack6();
                break;
            case 7:
                Attack7();
                break;
            case 8:
                Attack8();
                break;
            case 9:
                Attack9();
                break;
        }
    }

    private void OffLasers()
    {
        LaserGunController[] laserGuns = FindObjectsByType<LaserGunController>(FindObjectsSortMode.None);

        foreach (LaserGunController laserGun in laserGuns)
        {
            if (laserGun != null) laserGun.Off();
        }

        _numLasers = 0;
    }

    private void SetLasersForSimpleAttack(int gapHor, int gapVer, int numToIntervalHor, int numToIntervalVer, int intervalHor = 1, int intervalVer = 1)
    {
        int interval = 0;
        int gap = 0;

        for (int i = gapHor; i < _laserGunControllerHor.Length; i++)
        {
            if (interval < numToIntervalHor)
            {
                interval++;
                _laserGuns[_numLasers++] = _laserGunControllerHor[i];
            } else
            {
                gap++;
                if (gap >= intervalHor)
                {
                    interval = 0;
                    gap = 0;
                }

            }
        }

        interval = 0;
        gap = 0;
        for (int i = gapVer; i < _laserGunControllerVer.Length; i++)
        {
            if (interval < numToIntervalVer)
            {
                interval++;
                _laserGuns[_numLasers++] = _laserGunControllerVer[i];
            } else
            {
                gap++;
                if (gap >= intervalVer)
                {
                    interval = 0;
                    gap = 0;
                }
            }
        }
    }

    private void Warning()
    {
        Vector3[] warningPos = new Vector3[2000];
        int numWarning = 0;

        for (int i = 0; i < _numLasers; i++)
        {
            Vector3[] laserPositions = _laserGuns[i].Lasers();

            for (int j = 1; j < laserPositions.Length; j++)
            {
                warningPos[numWarning + j] = laserPositions[j];
                _warningBlocks[numWarning + j] = Instantiate(_warningBlock, laserPositions[j], Quaternion.AngleAxis(0, Vector3.forward));
            }

            numWarning += laserPositions.Length;
        }
    }

    private void DeactivatedWarning()
    {
        GameObject[] warnings = GameObject.FindGameObjectsWithTag("Warning");

        foreach (GameObject warning in warnings)
        {
            Destroy(warning);
        }

        for (int i = 0; i < _warningBlocks.Length; i++)
        {
            if (_warningBlocks[i] != null)
            {
                Destroy(_warningBlocks[i]);
                _warningBlocks[i] = null;
            }
        }


    }

    private void SpawnLasers()
    {
        DeactivatedWarning();

        for (int i = 0; i < _numLasers; i++)
        {
            _laserGuns[i].On();
        }

        _numLasers = 0;
    }

    private LaserGunController[] SortedLaserGuns(LaserGunController[] laserGuns)
    {

        System.Array.Sort(laserGuns, (a, b) =>
        {
            int cmp = a.transform.position.x.CompareTo(b.transform.position.x);
            if (cmp == 0)
                cmp = a.transform.position.y.CompareTo(b.transform.position.y);
            return cmp;
        });

        return laserGuns;
    }

    private void Die()
    {
        EventController.Instance.EventTriggerActivated(1);
        Player.Instance.AddSomeHP();
        OffLasers();
        _isDied = true;
    }

    private void Attack0(int variant)
    {
        switch (_stageAttack)
        {
            case 1:
                _timer = 0.7f;
                SetLasersForSimpleAttack(variant, variant, 1, 1);
                Warning();
                break;
            case 2:
                SpawnLasers();
                break;
            case 3:
                OffLasers();
                SetLasersForSimpleAttack(1 - variant, 1 - variant, 1, 1);
                Warning();
                break;
            case 4:
                SpawnLasers();
                break;
            case 5:
                OffLasers();
                _stageAttack = 0;
                _numAttacks++;
                _timer = 0.5f;
                break;
        }
    }

    private void Attack1()
    {
        switch (_stageAttack)
        {
            case 1:
                SetLasersForSimpleAttack(12, 2, 1, 1, 2, 2);
                Warning();
                break;
            case 2:
                SpawnLasers();
                SetLasersForSimpleAttack(12, 1, 1, 1, 2, 2);
                Warning();
                break;
            case 3:
                OffLasers();
                SetLasersForSimpleAttack(12, 1, 1, 1, 2, 2);
                SpawnLasers();
                SetLasersForSimpleAttack(12, 0, 1, 1, 2, 2);
                Warning();
                break;
            case 4:
                OffLasers();
                SetLasersForSimpleAttack(12, 0, 1, 1, 2, 2);
                SpawnLasers();
                break;
            case 5:
                OffLasers();
                _stageAttack = 0;
                _numAttacks++;
                break;
        }
    }

    private void Attack2()
    {
        switch (_stageAttack)
        {
            case 1:
                SetLasersForSimpleAttack(0, 12, 1, 1, 2, 2);
                Warning();
                break;
            case 2:
                SpawnLasers();
                SetLasersForSimpleAttack(1, 12, 1, 1, 2, 2);
                Warning();
                break;
            case 3:
                OffLasers();
                SetLasersForSimpleAttack(1, 12, 1, 1, 2, 2);
                SpawnLasers();
                SetLasersForSimpleAttack(2, 12, 1, 1, 2, 2);
                Warning();
                break;
            case 4:
                OffLasers();
                SetLasersForSimpleAttack(2, 12, 1, 1, 2, 2);
                SpawnLasers();
                break;
            case 5:
                OffLasers();
                _stageAttack = 0;
                _numAttacks++;
                break;
        }
    }

    private void Attack3()
    {
        switch (_stageAttack)
        {
            case 1:
                SetLasersForSimpleAttack(2, 12, 2, 1);
                Warning();
                break;
            case 2:
                SpawnLasers();
                SetLasersForSimpleAttack(1, 12, 2, 1);
                Warning();
                break;
            case 3:
                OffLasers();
                SetLasersForSimpleAttack(1, 12, 2, 1);
                break;
            case 4:
                SpawnLasers();
                SetLasersForSimpleAttack(0, 12, 2, 1);
                Warning();
                break;
            case 5:
                OffLasers();
                SetLasersForSimpleAttack(0, 12, 2, 1);
                break;
            case 6:
                SpawnLasers();
                break;
            case 7:
                OffLasers();
                _stageAttack = 0;
                _numAttacks++;
                break;
        }
    }

    private void Attack4()
    {
        switch (_stageAttack)
        {
            case 1:
                SetLasersForSimpleAttack(12, 0, 2, 1);
                Warning();
                break;
            case 2:
                SpawnLasers();
                SetLasersForSimpleAttack(12, 1, 2, 1);
                Warning();
                break;
            case 3:
                OffLasers();
                SetLasersForSimpleAttack(12, 1, 2, 1);
                break;
            case 4:
                SpawnLasers();
                SetLasersForSimpleAttack(12, 2, 2, 1);
                Warning();
                break;
            case 5:
                OffLasers();
                SetLasersForSimpleAttack(12, 2, 2, 1);
                break;
            case 6:
                SpawnLasers();
                break;
            case 7:
                OffLasers();
                _stageAttack = 0;
                _numAttacks++;
                break;
        }
    }

    private void Attack5()
    {
        switch (_stageAttack)
        {
            case 1:
                SetLasersForSimpleAttack(0, 0, 1, 1, 1, 10);
                Warning();
                break;
            case 2:
                SpawnLasers();
                SetLasersForSimpleAttack(0, 0, 1, 2, 1, 8);
                Warning();
                break;
            case 3:
                SpawnLasers();
                SetLasersForSimpleAttack(0, 0, 1, 3, 1, 6);
                Warning();
                break;
            case 4:
                SpawnLasers();
                SetLasersForSimpleAttack(0, 0, 1, 4, 1, 4);
                Warning();
                break;
            case 5:
                SpawnLasers();
                SetLasersForSimpleAttack(0, 0, 1, 5, 1, 2);
                Warning();
                break;
            case 6:
                SpawnLasers();
                break;
            case 7:
                SpawnLasers();
                OffLasers();
                _stageAttack = 0;
                _numAttacks++;
                break;
        }
    }

    private void Attack6()
    {
        switch (_stageAttack)
        {
            case 1:
                SetLasersForSimpleAttack(0, 0, 1, 1, 10, 1);
                Warning();
                break;
            case 2:
                SpawnLasers();
                SetLasersForSimpleAttack(0, 0, 2, 1, 8, 1);
                Warning();
                break;
            case 3:
                SpawnLasers();
                SetLasersForSimpleAttack(0, 0, 3, 1, 6, 1);
                Warning();
                break;
            case 4:
                SpawnLasers();
                SetLasersForSimpleAttack(0, 0, 4, 1, 4, 1);
                Warning();
                break;
            case 5:
                SpawnLasers();
                SetLasersForSimpleAttack(0, 0, 5, 1, 2, 1);
                Warning();
                break;
            case 6:
                SpawnLasers();
                break;
            case 7:
                SpawnLasers();
                OffLasers();
                _stageAttack = 0;
                _numAttacks++;
                break;
        }
    }

    private void Attack7()
    {
        switch (_stageAttack)
        {
            case 1:
                SetLasersForSimpleAttack(12, 0, 1, 2, 2, 1);
                Warning();
                break;
            case 2:
                SetLasersForSimpleAttack(0, 0, 1, 2, 2, 1);
                Warning();
                break;
            case 3:
                SpawnLasers();
                SetLasersForSimpleAttack(1, 0, 1, 2, 2, 1);
                Warning();
                break;
            case 4:
                OffLasers();
                SetLasersForSimpleAttack(1, 0, 1, 2, 2, 1);
                SpawnLasers();
                SetLasersForSimpleAttack(2, 0, 1, 2, 2, 1);
                Warning();
                break;
            case 5:
                OffLasers();
                SetLasersForSimpleAttack(2, 0, 1, 2, 2, 1);
                SpawnLasers();
                break;
            case 6:
                OffLasers();
                _stageAttack = 0;
                _numAttacks++;
                break;
        }
    }

    private void Attack8()
    {
        switch (_stageAttack)
        {
            case 1:
                SetLasersForSimpleAttack(12, 1, 1, 2, 2, 1);
                Warning();
                break;
            case 2:
                SetLasersForSimpleAttack(2, 1, 1, 2, 2, 1);
                Warning();
                break;
            case 3:
                SpawnLasers();
                SetLasersForSimpleAttack(1, 1, 1, 2, 2, 1);
                Warning();
                break;
            case 4:
                OffLasers();
                SetLasersForSimpleAttack(1, 1, 1, 2, 2, 1);
                SpawnLasers();
                SetLasersForSimpleAttack(0, 1, 1, 2, 2, 1);
                Warning();
                break;
            case 5:
                OffLasers();
                SetLasersForSimpleAttack(0, 1, 1, 2, 2, 1);
                SpawnLasers();
                break;
            case 6:
                OffLasers();
                _stageAttack = 0;
                _numAttacks++;
                break;
        }
    }

    private void Attack9()
    {
        switch (_stageAttack)
        {
            case 1:
                SetLasersForSimpleAttack(1, 12, 2, 1, 1, 2);
                Warning();
                break;
            case 2:
                SetLasersForSimpleAttack(1, 0, 2, 1, 1, 2);
                Warning();
                break;
            case 3:
                SpawnLasers();
                SetLasersForSimpleAttack(1, 1, 2, 1, 1, 2);
                Warning();
                break;
            case 4:
                OffLasers();
                SetLasersForSimpleAttack(1, 1, 2, 1, 1, 2);
                SpawnLasers();
                SetLasersForSimpleAttack(1, 2, 2, 1, 1, 2);
                Warning();
                break;
            case 5:
                OffLasers();
                SetLasersForSimpleAttack(1, 2, 2, 1, 1, 2);
                SpawnLasers();
                break;
            case 6:
                OffLasers();
                _stageAttack = 0;
                _numAttacks++;
                break;
        }
    }
}


