using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCheck : MonoBehaviour
{
    private string cTag = "Edge"; //�Ώۃ^�O
    private bool isCheck = false; //�ΏۂƂ̐ڐG����t���O
    private bool isCheckEnter, isCheckStay, isCheckExit; //���ضް3�킻�ꂼ��̔���t���O

    /// <summary>
    /// �ڐG����
    /// </summary>
    /// <returns></returns>
    public bool IsEdge()
    {
        if (isCheckEnter || isCheckStay) //�ڐG�������A�ڐG�������Ă���ꍇ
        {
            isCheck = true; //�ڒn����g�D���[
        }
        else if (isCheckExit) //�ڐG�𗣂ꂽ�ꍇ
        {
            isCheck = false; //�ڒn����t�@���X
        }

        //3��t���O�̃��Z�b�g
        isCheckEnter = false; //�ڐG���t���O
        isCheckStay = false; //�ڐG���t���O
        isCheckExit = false; //�ڐG�����t���O

        return isCheck; //���肵���ڐG�����Ԃ�
    }

    /// <summary>
    /// �ΏۂƐڐG��
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == cTag)
        {
            isCheckEnter = true; //�ڐG���t���O���g�D���[
        }
    }

    /// <summary>
    /// �ΏۂƐڐG�������Ă���
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == cTag)
        {
            isCheckStay = true; //�ڐG���t���O���g�D���[
        }
    }

    /// <summary>
    /// �Ώۂ��痣�ꂽ
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == cTag)
        {
            isCheckExit = true; //�ڐG�����t���O���g�D���[
        }
    }
}
