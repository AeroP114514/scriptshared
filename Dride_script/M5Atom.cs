using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;


public class M5Atom : MonoBehaviour
{

    [SerializeField]
    string serialPortName = "COM1";     // ���O�̓f�o�C�X�}�l�[�W���[�̃|�[�g USB Serial Port �̃J�b�R�̒�

    // �ʐM���x M5StackAtom�ƍ��킹��
    readonly int serialPortBaudRate = 115200;

    // �X���̃X�P�[��
    [SerializeField]
    float rotScale = 10;

    // �V���A���ʐM�p
    SerialPort serialPort;

    bool isRunning_ = false;
    bool isUpdate_ = false;
    Thread thread_;

    float deg, dot;


    void Start()
    {
        OpenSerialPort();                   // �V���A���|�[�g�J��

        isRunning_ = true;
        isUpdate_  = false;
        thread_ = new Thread(ReadSerialPort);
        thread_.Start();
    }

    void Update()
    {
        if(isUpdate_)
        {
            // �f�[�^�͕�����Ȃ̂ŁA���l�ɒ����ČX���̒l�Ƃ���
            float x = 0;//deg * rotScale;
            float y = dot * rotScale;
            float z = 0;

            // �Q�[���I�u�W�F�N�g�ɔ��f����
            Quaternion rotation = Quaternion.Euler(x, z, y);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotation, 25f);

            isUpdate_ = false;
        }
    }

    // �V���A���ʐM�� M5StackAtom�� �ʐM�J�n
    void OpenSerialPort()
    {
        serialPort = new SerialPort(serialPortName, serialPortBaudRate);
        serialPort.Open();
    }

    // �V���A���ʐM�Ńf�[�^�𓾂�
    void ReadSerialPort()
    {
        while (isRunning_ && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                if (serialPort.BytesToRead > 0)
                {
                    string[] data = serialPort.ReadLine().Split(',');
                    deg = float.Parse(data[0]);
                    dot = float.Parse(data[1]);
                    isUpdate_ = true;
                    //message_ = serialPort_.ReadLine();
                    //isNewMessageReceived_ = true;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }

        //if (!serialPort.IsOpen){ return;}

        // �����Ă����f�[�^�����o��

        //string[] data = serialPort.ReadLine().Split(',');
        //float deg = float.Parse(data[0]);
        //float dot = float.Parse(data[1]);

        /*string dx = data.Substring(1, data.IndexOf("y") - 1);
        string dy = data.Substring(data.IndexOf("y") + 1);
        dy = dy.Substring(0, dy.IndexOf("z") - 1);
        string dz = data.Substring(data.IndexOf("z") + 1);
        */
    }
}

