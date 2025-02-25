using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour, IBattle, IHealth
{
    /// <summary>
    /// 무기에 붙어있는 파티클 시스템 컴포넌트
    /// </summary>
    ParticleSystem weaponPS;

    /// <summary>
    /// 무기가 붙어있을 게임오브젝트의 트랜스폼
    /// </summary>
    Transform weapon_r;

    /// <summary>
    /// 방패가 붙어있을 게임 오브젝트의 트랜스폼
    /// </summary>
    Transform weapon_l;

    /// <summary>
    /// 무기가 데미지를 주는 영역의 트리거
    /// </summary>
    Collider weaponBlade;

    Animator anim;  // 애니메이터 컴포넌트

    public float attackPower = 10.0f;      // 공격력
    public float defencePower = 3.0f;      // 방어력
    public float maxHP = 100.0f;    // 최대 HP
    float hp = 100.0f;              // 현재 HP
    bool isAlive = true;            // 살았는지 죽었는지 확인용


    public float itemPickupRange = 2.0f; 

    // 프로퍼티 ------------------------------------------------------------------------------------
    public float AttackPower => attackPower;

    public float DefencePower => defencePower;

    public float HP
    {
        get => hp;
        set
        {
            if (isAlive && hp != value) // 살아있고 HP가 변경되었을 때만 실행
            {
                hp = value;

                if(hp < 0)
                {
                    Die();
                }

                hp = Mathf.Clamp(hp, 0.0f, maxHP);

                onHealthChange?.Invoke(hp/maxHP);
            }
        }
    }

    // 프로퍼티 ------------------------------------------------------------------------------------
    public float MaxHP => maxHP;
    public bool IsAlive => isAlive;

    // 델리게이트 ----------------------------------------------------------------------------------
    public Action<float> onHealthChange { get; set; }
    public Action onDie { get; set; }
    // --------------------------------------------------------------------------------------------

    private void Awake()
    {
        anim = GetComponent<Animator>();

        weapon_r = GetComponentInChildren<WeaponPosition>().transform;  // 무기가 붙는 위치를 컴포넌트의 타입으로 찾기
        weapon_l = GetComponentInChildren<ShildPosition>().transform;   // 방패가 붙는 위치를 컴포넌트의 타입으로 찾기

        // 장비교체가 일어나면 새로 설정해야 한다.
        weaponPS = weapon_r.GetComponentInChildren<ParticleSystem>();   // 무기에 붙어있는 파티클 시스템 가져오기
        weaponBlade = weapon_r.GetComponentInChildren<Collider>();      // 무기의 충돌 영역 가져오기
    }

    private void Start()
    {
        hp = maxHP;
        isAlive = true;
    }

    /// <summary>
    /// 무기의 이팩트를 키고 끄는 함수
    /// </summary>
    /// <param name="on">true면 무기 이팩트를 켜고, flase면 무기 이팩트를 끈다.</param>
    public void WeaponEffectSwitch(bool on)
    {
        if( weaponPS != null )
        {
            if(on)
            {
                weaponPS.Play();    // 파티클 이팩트 재생 시작
            }
            else
            {
                weaponPS.Stop();    // 파티클 이팩트 재생 중지
            }
        }
    }

    /// <summary>
    /// 무기가 공격 행동을 할 때 무기의 트리거 켜는 함수
    /// </summary>
    public void WeaponBladeEnable()
    {
        if(weaponBlade!=null)
        {
            weaponBlade.enabled = true;
        }
    }

    /// <summary>
    /// 무기가 공격 행동이 끝날 때 무기의 트리거를 끄는 함수
    /// </summary>
    public void WeaponBladeDisable()
    {
        if (weaponBlade != null)
        {
            weaponBlade.enabled = false;
        }
    }

    /// <summary>
    /// 무기와 방패를 표시하거나 표시하지 않는 함수
    /// </summary>
    /// <param name="isShow">ture면 표시하고, false면 표시하지 않는다.</param>
    public void ShowWeaponAndSheild(bool isShow)
    {
        weapon_r.gameObject.SetActive(isShow);
        weapon_l.gameObject.SetActive(isShow);
    }

    /// <summary>
    /// 공격용 함수
    /// </summary>
    /// <param name="target">공격할 대상</param>
    public void Attack(IBattle target)
    {
        target?.Defence(AttackPower);
    }

    /// <summary>
    /// 방어용 함수
    /// </summary>
    /// <param name="damage">현재 입은 데미지</param>
    public void Defence(float damage)
    {
        if (isAlive)                // 살아있을 때만 데미지 입음.
        {
            anim.SetTrigger("Hit"); // 피격 애니메이션 재생            
            HP -= (damage - DefencePower);  // 기본 공식 : 실제 입는 데미지 = 적 공격 데미지 - 방어력
        }
    }

    /// <summary>
    /// 죽었을 때 실행될 함수
    /// </summary>
    public void Die()
    {
        isAlive = false;
        ShowWeaponAndSheild(true);
        anim.SetLayerWeight(1, 0.0f);       // 애니메이션 레이어 가중치 제거
        anim.SetBool("IsAlive", isAlive);   // 죽었다고 표시해서 사망 애니메이션 재생
        onDie?.Invoke();
    }

    /// <summary>
    /// 플레이어 주변의 아이템을 획득하는 함수
    /// </summary>
    public void ItemPickup()
    {
        Collider[] items = Physics.OverlapSphere(transform.position, itemPickupRange, LayerMask.GetMask("Item"));

        foreach(var item in items)
        {
            Destroy(item.gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.up, itemPickupRange);
    }
#endif


}
