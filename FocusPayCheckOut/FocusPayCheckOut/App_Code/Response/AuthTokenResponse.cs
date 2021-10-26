
#region Namespaces

using System;
using System.Xml.Serialization;

#endregion

namespace TF.FocusPayCheckOut.Response
{
    #region AuthTokenResponse

    [Serializable]
    [XmlType("Envelope")]
    public class AuthTokenResponse
    {
        private AuthTokenBody objBody;

        #region Body
      
        [XmlElement(ElementName = "Body")]
        public AuthTokenBody Body
        {
            get { return objBody; }
            set { objBody = value; }
        }
        #endregion
    }

    [Serializable]
    [XmlType]
    public class AuthTokenBody
    {
        private GetNewTokenResponse authorizeTokenResponse;

        #region AuthorizePaymentCardResponse
      
        [XmlElement(ElementName = "GetNewTokenResponse")]
        public GetNewTokenResponse AuthorizeTokenResponse
        {
            get { return authorizeTokenResponse; }
            set { authorizeTokenResponse = value; }
        }
        #endregion
    }

    [Serializable]
    [XmlType]
    public class GetNewTokenResponse
    {
        private GetNewTokenResult tokenResponse;

        #region AuthorizePaymentCardResponse

        [XmlElement(ElementName = "GetNewTokenResult")]
        public GetNewTokenResult TokenResponse
        {
            get { return tokenResponse; }
            set { tokenResponse = value; }
        }
        #endregion
    }

    [Serializable]
    [XmlType]
    public class GetNewTokenResult : Response
    {
        #region Member Variables

        //email.
        private string email;

        //token.
        private string token;

        //active
        private string active;

        //tokenExpiration.
        private string tokenDtExpiration;

        //tokenLastUpdate.
        private string tokenDtLastUpdate;

        //dtOriginalIssue
        private string dtOriginalIssue;

        //dtServerTime
        private string dtServerTime;

        #endregion

        #region Properties

        #region Email

        /// <summary>
        /// Allows get/set the Email.
        /// </summary>
        [XmlElement(ElementName = "Email")]
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                this.email = value;
            }
        }

        #endregion

        #region Token

        /// <summary>
        /// Allows get/set the Token.
        /// </summary>
        [XmlElement(ElementName = "Token")]
        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                this.token = value;
            }
        }

        #endregion

        #region Active

        /// <summary>
        /// Allows get/set the Active
        /// </summary>
        [XmlElement(ElementName = "bActive")]
        public string Active
        {
            get
            {
                return active;
            }
            set
            {
                this.active = value;
            }
        }

        #endregion

        #region TokenDtExpiration

        /// <summary>
        /// Allows get/set the TokenDtExpiration
        /// </summary>
        [XmlElement(ElementName = "dtExpiration")]
        public string TokenDtExpiration
        {
            get
            {
                return tokenDtExpiration;
            }
            set
            {
                this.tokenDtExpiration = value;
            }
        }

        #endregion

        #region TokenDtLastUpdate

        /// <summary>
        /// Allows get/set the TokenDtLastUpdate.
        /// </summary>     
        [XmlElement(ElementName = "dtLastUpdate")]
        public string TokenDtLastUpdate
        {
            get
            {
                return tokenDtLastUpdate;
            }
            set
            {
                this.tokenDtLastUpdate = value;
            }
        }

        #endregion

        #region DtOriginalIssue

        /// <summary>
        /// Allows get/set the DtOriginalIssue.
        /// </summary>
        [XmlElement(ElementName = "dtOriginalIssue")]
        public string DtOriginalIssue
        {
            get
            {
                return dtOriginalIssue;
            }
            set
            {
                this.dtOriginalIssue = value;
            }
        }

        #endregion

        #region DtServerTime

        /// <summary>
        /// Allows get/set the DtServerTime.
        /// </summary>
        [XmlElement(ElementName = "dtServerTime")]
        public string DtServerTime
        {
            get
            {
                return dtServerTime;
            }
            set
            {
                this.dtServerTime = value;
            }
        }

        #endregion

        #endregion
    }

    #endregion      
}
