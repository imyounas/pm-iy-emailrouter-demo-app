using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Models.Email
{
    public class PostProcessedMetaData
    {
        public PostProcessedMetaData()
        {
            PriorityScore = 10; //1 to 10 being heighest 
        }

        private float _messageReputationScore;
        private float _senderReputationScore;
        private EPMTnCRatings _pmTnCRatings;

        // Post Prcessing
        public float FilteredRecepientsPercentage { get; set; }
        public List<string> FilteredRecepients { get; set; }
        public int PriorityScore { get; set; }
        public float SenderReputationScore
        {
            get => _senderReputationScore;
            set
            {
                _senderReputationScore = value;
                if (_senderReputationScore < 50)
                {
                    PriorityScore -= 2;
                }

            }
        }
        public float MessageReputationScore
        {
            get => _messageReputationScore;
            set
            {
                _messageReputationScore = value;
                if (_messageReputationScore < 50)
                {
                    PriorityScore -= 2;
                }
            }
        }
        public bool IsInviolationOfTnC { get; set; }
        public EPMTnCRatings PMTnCRatings
        {
            get => _pmTnCRatings;
            set
            {
                _pmTnCRatings = value;

                if (_pmTnCRatings == EPMTnCRatings.InAppropriate || _pmTnCRatings == EPMTnCRatings.Offensive)
                {
                    IsInviolationOfTnC = true;
                }
            }
        }
    }
}
