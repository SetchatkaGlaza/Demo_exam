using infoTech.Entity;
using System;
using System.Linq;
using System.Windows;

namespace infoTech
{
    public partial class CommentsWindow : Window
    {
        private infoTechEntities db = new infoTechEntities();
        private int requestId;

        public CommentsWindow(int requestId)
        {
            InitializeComponent();
            this.requestId = requestId;
            LoadComments();
            LoadRequestInfo();
        }

        private void LoadRequestInfo()
        {
            var request = db.Requests.Find(requestId);
            if (request != null)
                lblRequestInfo.Content = $"Заявка №{requestId} – {request.problemDescription}";
        }

        private void LoadComments()
        {
            lbComments.ItemsSource = db.Comments
                .Where(c => c.requestID == requestId)
                .OrderBy(c => c.commentID)
                .ToList();
        }

        private void btnAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewComment.Text))
                return;

            var comment = new Comments
            {
                requestID = requestId,
                message = txtNewComment.Text.Trim(),
                masterID = App.CurrentUser?.masterID ?? 1 // если текущий мастер есть
            };

            db.Comments.Add(comment);
            db.SaveChanges();
            txtNewComment.Clear();
            LoadComments();
        }
    }
}