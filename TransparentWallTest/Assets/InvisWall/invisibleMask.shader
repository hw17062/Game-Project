Shader "Custom/invisibleMask"
{
    //Shader used form here: https://answers.unity.com/questions/316064/can-i-obscure-an-object-using-an-invisible-object.html

   SubShader {
    Tags {"Queue"="Transparent+1"}
    Pass {
        Blend Zero One
    }
   }
}
