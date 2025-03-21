# 프로젝트 이름

**SpartaDungeonExpedition**

## 📖 목차
1. [프로젝트 소개](#프로젝트-소개)
2. [팀소개](#팀소개)
3. [프로젝트 계기](#프로젝트-계기)
4. [주요기능](#주요기능)
5. [개발기간](#개발기간)
6. [기술스택](#기술스택)
    
## 👨‍🏫 프로젝트 소개
내일배움캠프 Unity 7기 스파르타 던전 탐험 만들기 개인과제입니다.

## 팀소개
//Only Me

## 프로젝트 계기
Unity3D에 대해 학습하고, Unity3D의 캐릭터 이동과 물리 처리를 직접 구현하기 위해 만들었습니다. 

## 💜 주요기능

**- 기능 1
기본 이동 및 점프, 대쉬**

WASD, SpaceBar, LeftShift 키 입력을 통해 캐릭터가 이동할 수 있습니다. 

**- 기능 2
상호작용**

RayCast를 이용해 물체를 감지하고, 만약 그 물체가 상호작용이 가능한 물체라면 물체의 정보를 표시합니다.

**- 기능 3
UI**
1. 캐릭터 상태UI
   
캐릭터의 컨디션(체력, 허기, 스태미나)을 UI에 표시하였고, 시간이 지날수록 감소하거나 줄어들며, 데미지를 입으면 체력이 감소하고, 공격을 하거나 대쉬를 하면 스태미나가 감소합니다.

2. UIInventory
Tab키를 눌러 인벤토리창을 열수 있습니다.

인벤토리가 열린 동안에는 카메라 전환이 불가합니다.

상호작용을 통해 현재 보유하고 있는 아이템 목록을 확인할 수 있으며, 아이템 타입에 따라 소모품이면 사용하기, 장비면 장착 및 해제가 가능합니다.

아이템 타입과 관계없이 버리기는 항상 가능합니다. 

**- 기능 4
플랫폼**

점프 플랫폼 :

rigidbody.velocity를 사용하여 점프 효과를 부드럽게 만들었습니다.

좌우로 움직이는 플랫폼 :

일정한 구역 내에서 좌우로 이동하는 플랫폼입니다.

캐릭터가 플랫폼 위에 탔을 때 떨어지지 않고, 옆면에 부딪혔을 때 플랫폼에 충돌처리가 되지 않도록 예외처리를 하였습니다. 

**-기능 5
DamageObject**

1. NPC

AI Navigation을 통해 walkable 구역 내에서 배회하며, 일정 거리 내에서 플레이어가 감지되면 쫓아와 공격하도록 만들었습니다. 

2. CampFire

IDamageable을 상속받아 플레이어가 가까이 가면 플레이어에게 대미지를 입힙니다. 

## ⏲️ 개발기간
- 25.03.04(화) ~ 25.03.11(화)

## 📚️ 기술스택

### ✔️ Language
C#

### ✔️ Version Control
Githup Desktop

### ✔️ IDE
Visual Studio

### ✔️ Framework
Unity Engine
