using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace fpga.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<string> Ports { get; } = new();
    public ObservableCollection<int> BaudRates { get; } = new() {115200,230400,460800,921600};

    string _filePath="";
    string _status="未选择文件";
    string _log="等待操作...";
    double _progress;

    public string FilePath { get=>_filePath; set{_filePath=value;OnPropertyChanged();} }
    public string? SelectedPort { get; set; }
    public int SelectedBaudRate { get; set; } = 921600;
    public string Timeout { get; set; } = "2.0";
    public string FlashBaseAddress { get; set; } = "0x400000";
    public bool QueryStatusAfterPacket { get; set; } = true;

    public string Status { get=>_status; set{_status=value;OnPropertyChanged();} }
    public string LogText { get=>_log; set{_log=value;OnPropertyChanged();} }
    public double Progress { get=>_progress; set{_progress=value;OnPropertyChanged();} }

    public ICommand BrowseFileCommand { get; }
    public ICommand RefreshPortsCommand { get; }
    public ICommand QueryVersionCommand { get; }
    public ICommand UpgradeCommand { get; }
    public ICommand ClearLogCommand { get; }

    public MainViewModel()
    {
        BrowseFileCommand = new RelayCommand(_ => BrowseFile());
        RefreshPortsCommand = new RelayCommand(_ => RefreshPorts());
        QueryVersionCommand = new RelayCommand(_ => AppendLog("查询版本(示例)..."));
        UpgradeCommand = new RelayCommand(_ => StartUpgrade());
        ClearLogCommand = new RelayCommand(_ => LogText = "");

        RefreshPorts();
    }

    void BrowseFile()
    {
        var dlg = new OpenFileDialog();
        dlg.Filter = "FPGA文件|*.bin;*.bit;*.rbf|所有文件|*.*";
        if (dlg.ShowDialog() == true)
        {
            FilePath = dlg.FileName;
            AppendLog($"选择文件: {dlg.FileName}");
        }
    }

    void RefreshPorts()
    {
        Ports.Clear();
        foreach (var p in new[] { "COM1", "COM2" }) //SerialPort Model
            Ports.Add(p);
        AppendLog("已刷新串口列表");
    }

    void StartUpgrade()
    {
        Status = "升级中";
        Progress = 10;
        AppendLog("开始升级...");
    }

    void AppendLog(string msg)
    {
        LogText += $"[{DateTime.Now:HH:mm:ss}] {msg}\n";
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName]string? n=null)
        => PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(n));
}
