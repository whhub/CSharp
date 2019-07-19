using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RequisitionValidation
{
    class Program
    {
        private static WorkflowServiceWebReference.WorkflowService _workflowService;
        private static int _userId;

        static void Main(string[] args)
        {
            _workflowService = new WorkflowServiceWebReference.WorkflowService();
            while(true)
            {
                ValidateRequisitions();
                Thread.Sleep(1000*60);
            }
        }

        private static void ValidateRequisitions()
        {
            Console.WriteLine("{0} validating...", DateTime.Now);

            //轮询某表（流程id, 流程名称, 流程当前状态，审批人）
            var requests = QueryRequisitons();
            foreach(var request in requests)
            {
                //调用对应的 validate接口，返回值{isSuccess, errorMsg}.
                int requestId = Convert.ToInt32(request.requestId);
                string errorMsg;
                bool isSuccess = ValidateRequisition(requestId, out errorMsg);

                //isSuccess为true, 调用上述OA接口2；反之调用OA接口3并将errorMsg更新到签字意见中去.
                if (isSuccess)
                {
                    SubmitRequisition(requestId);
                    //todo: 当流程当前状态为已完成时，调用OA接口1获取表单数据，然后调用MES的存储过程。
                    bool isArchived = true;
                    if(isArchived)
                    {
                        StoreInMes(requestId);
                    }
                }
                else
                {
                    GiveBackRequisition(requestId,errorMsg);
                }

            }
        }

        private static WorkflowServiceWebReference.WorkflowRequestInfo[] QueryRequisitons()
        {
            var requests = _workflowService.getToDoWorkflowRequestList(0, 100, 1000, 257, new string[] { string.Empty });
            return requests;
        }

        private static bool ValidateRequisition(int requisitionId, out string errorMsg)
        {
            if(requisitionId % 2 != 0) {
                errorMsg = "error";
                return false;
            }
            else
            {
                errorMsg = string.Empty;
                return true;
            }
        }

        private static void SubmitRequisition(int requisitionId)
        {
            var requestInfo = GetRequisition(requisitionId);
            _workflowService.submitWorkflowRequest(requestInfo, requisitionId, _userId, "submit", "同意");
        }

        private static void GiveBackRequisition(int requisitionId, string errorMessage)
        {
            var requestInfo = GetRequisition(requisitionId);
            _workflowService.submitWorkflowRequest(requestInfo, requisitionId, _userId, "reject", errorMessage);
        }

        private static WorkflowServiceWebReference.WorkflowRequestInfo GetRequisition(int requisitionId)
        {
            return _workflowService.getWorkflowRequest(requisitionId, _userId, 0);
        }

        private static void StoreInMes(int requisitionId)
        {
            var requestInfo = GetRequisition(requisitionId);
            // store into MES
        }
    }
}
