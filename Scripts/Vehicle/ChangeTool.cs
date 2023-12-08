using System;
using _Project.Scripts;
using Obi;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class ChangeTool : MonoBehaviour
{
   public Crane crane;
   [Title("Pull Obj")]
   [SerializeField] private Collider pullCollider;
   [SerializeField] private MeshRenderer pullMeshRenderer;
   [SerializeField] private Material pullEnabledMat;
   [SerializeField] private GameObject vacuumParticle;
   
   [Title("Ball Obj")]
   [SerializeField] private Collider ballCollider;
   [SerializeField] private MeshRenderer ballMeshRenderer;
   [SerializeField] private Material ballEnabledMat;
   
   [Title("Obi Rope")]
   [SerializeField] private Material ropeEnabledMat;
   [SerializeField] private Material ropeDisabledMat;
   [SerializeField] private MeshRenderer ropeMeshRenderer;
   
   [Title("Obi Rode")]
   [SerializeField] private MeshRenderer rodeMeshRenderer;
   
   
   [Title("Buttons")] 
   [SerializeField] protected Button changeToolButton;
   [SerializeField] protected Button exitAreaButton;
   
   [Title("ButtonsSprite")] 
   [SerializeField] private GameObject vacuumSprite;
   [SerializeField] private GameObject ballSprite;
   [SerializeField] private TextMeshProUGUI textToolChangeButton;

   
   
   private bool _switch;

   private void Init()
   {
      _switch = false;
      textToolChangeButton.text = "Switch to Vacuum";
      vacuumSprite.gameObject.SetActive(true);
      ballSprite.gameObject.SetActive(false);
      //ballObj.SetActive(true);
      ballCollider.enabled = true;
      ballMeshRenderer.material = ballEnabledMat;
      ropeMeshRenderer.material = ropeEnabledMat;
      
      //pullObj.SetActive(false);
      pullCollider.enabled = false;
      pullMeshRenderer.material = ropeDisabledMat;
      vacuumParticle.SetActive(false);
      rodeMeshRenderer.material = ropeDisabledMat;

      
   }

   private void OnEnable()
   {
      Init();
      changeToolButton.onClick.AddListener(Tool);
      exitAreaButton.onClick.AddListener(ExitArea);
      GameEventPool.OnMobileJoystickDown+= DisableButtons;
      GameEventPool.OnMobileJoystickUp += EnableButtons;
   }

   private void OnDisable()
   {
      changeToolButton.onClick.RemoveListener(Tool);
      exitAreaButton.onClick.RemoveListener(ExitArea);
      GameEventPool.OnMobileJoystickDown-= DisableButtons;
      GameEventPool.OnMobileJoystickUp -= EnableButtons;
   }

   protected virtual void EnableButtons()
   {
       changeToolButton.gameObject.SetActive(true);
       exitAreaButton.gameObject.SetActive(true);
      
   }
   private void DisableButtons()
   {
      changeToolButton.gameObject.SetActive(false);
      exitAreaButton.gameObject.SetActive(false);
   }
   
   private void Tool()
   {
      _switch = !_switch;
     
      if (_switch)
      {
         //ballObj.SetActive(false);
         ballCollider.enabled = false;
         ballMeshRenderer.material = ropeDisabledMat;
         ropeMeshRenderer.material = ropeDisabledMat;
         //pullObj.SetActive(true);
         pullCollider.enabled = true;
         pullMeshRenderer.material = pullEnabledMat;
         vacuumParticle.SetActive(true);
         rodeMeshRenderer.material = ropeEnabledMat;
         textToolChangeButton.text = "Switch to Ball";
         vacuumSprite.gameObject.SetActive(false);
         ballSprite.gameObject.SetActive(true);


      }
      else
      {
        
         //ballObj.SetActive(true);
         ballCollider.enabled = true;
         ballMeshRenderer.material = ballEnabledMat;
         ropeMeshRenderer.material = ropeEnabledMat;
         pullCollider.enabled = false;
         vacuumParticle.SetActive(false);
         pullMeshRenderer.material = ropeDisabledMat;
         rodeMeshRenderer.material = ropeDisabledMat;
         textToolChangeButton.text = "Switch to Vacuum";
         vacuumSprite.gameObject.SetActive(true);
         ballSprite.gameObject.SetActive(false);
        
      }
      
   }
   protected virtual void ExitArea()
   {
      
   }
  
}