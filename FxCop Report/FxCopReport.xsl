<?xml version="1.0"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns="http://www.w3.org/TR/xhtml1/strict">
<xsl:key name="distinct-category" match="Message" use="@Category"/>
<xsl:key name="distinct-levels" match="Issue" use="@Level"/>

<xsl:template name="FxCopReport">
    <xsl:param name="Category" select="'NotAvailable'" />
    <xsl:for-each select="/FxCopReport">
      <tr bgcolor="white">
        <td>
          <xsl:value-of select="$Category" />
        </td>
        <td>
          <img border="0" src="App_Images/bar_critical_error.gif" height="20">
            <xsl:attribute name="width">
              <xsl:value-of select="count(.//Message[@Category =$Category]/Issue[@Level='CriticalError']) div 2"/>
            </xsl:attribute>
          </img>
          <img border="0" src="App_Images/bar_Critical_Warning.gif" height="20">
            <xsl:attribute name="width">
              <xsl:value-of select="count(.//Message[@Category =$Category]/Issue[@Level='CriticalWarning']) div 2"/>
            </xsl:attribute>
          </img>
          <img border="0" src="App_Images/bar_error.gif" height="20">
            <xsl:attribute name="width">
              <xsl:value-of select="count(.//Message[@Category =$Category]/Issue[@Level='Error']) div 2"/>
            </xsl:attribute>
          </img>
          <img border="0" src="App_Images/bar_warning.gif" height="20">
            <xsl:attribute name="width">
              <xsl:value-of select="count(.//Message[@Category =$Category]/Issue[@Level='Warning']) div 2"/>
            </xsl:attribute>
          </img>
          <img src="App_Images/space.gif" />
          <b>
            <xsl:value-of select="count(.//Message[@Category =$Category]/Issue[@Level!=''])"/>
          </b>
        </td>
        <td align="right">
          <xsl:value-of select="count(.//Message[@Category =$Category]/Issue[@Level='CriticalError'])"/>
        </td>
        <td align="right">
          <xsl:value-of select="count(.//Message[@Category =$Category]/Issue[@Level='CriticalWarning'])"/>
        </td>
        <td align="right">
          <xsl:value-of select="count(.//Message[@Category =$Category]/Issue[@Level='Error'])"/>
        </td>
        <td align="right">
          <xsl:value-of select="count(.//Message[@Category =$Category]/Issue[@Level='Warning'])"/>
        </td>
      </tr>
    </xsl:for-each>


  </xsl:template>

  <xsl:template name="FxCopRules" match="Rules">
    <xsl:param name="RuleID" select="'NotAvailable'" />
    
    <xsl:for-each select="/FxCopReport/Rules/Rule[@CheckId=$RuleID]">
        <b>Rule [<xsl:value-of select="$RuleID"/>] : </b><xsl:value-of select="Name/."/>
        <br />
      
      <b>Resolution : </b>
        <xsl:value-of select="Resolution/."/>
        <br /><br />
      
    </xsl:for-each>
    
  </xsl:template>
  
<xsl:template name="DetailFxCopReport">
    <xsl:param name="Category" select="'NotAvailable'" />
    <xsl:for-each select="/FxCopReport">
      <tr bgcolor="white">
        <td colspan="2">
          <a>
            <xsl:attribute name="href">
              javascript:ManageTreeMenu('icon_<xsl:value-of select = "substring($Category,11)" />', '<xsl:value-of select = "substring($Category,11)" />', true)
            </xsl:attribute>
            <b><span>
              <font color="red">
              <xsl:attribute name="id">
                <xsl:value-of select="concat('icon_', substring($Category,11))"/>
              </xsl:attribute>>
              </font>
            </span>
            <img border="0" src="App_Images/space.gif" width="10" />
              <xsl:value-of select="$Category" />
            </b>
          </a>
        </td>
      </tr>
      <tr>
        <td colspan="2">
          <span style="visibility:hidden;position:absolute">
            <xsl:attribute name="id">
              <xsl:value-of select="substring($Category,11)"/>
            </xsl:attribute>
            <xsl:for-each select="Targets/Target">
              <xsl:if test="count(.//Message[@Category =$Category]/Issue[@Level !='']) > 0">
                <table width="100%">
                  <tr bgcolor="white">
                    <td width="20"></td>
                    <td>
                      <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                          <td colspan="2">
                              <xsl:variable name="source-id" select="generate-id(.)"/>
                              <a>
                                <xsl:attribute name="href">
                                  javascript:ManageTreeMenu('<xsl:value-of select = "concat(substring($Category,11), '_', $source-id, '_Marker')" />', '<xsl:value-of select = "concat(substring($Category,11), '_', $source-id)" />', false)
                                </xsl:attribute>
                                <b><span>
                                  <font color="red">
                                    <xsl:attribute name="id">
                                      <xsl:value-of select="concat(substring($Category,11), '_', $source-id, '_Marker')"/>
                                    </xsl:attribute>>
                                  </font>
                                </span></b>
                                Assembly: <xsl:value-of select="@Name" /> 
                                <font color="black">
                                  <b> ( <xsl:value-of select="count(.//Message[@Category =$Category]/Issue[@Level!=''])"/> )</b>
                                </font>
                              </a>
                          
                          </td>
                        </tr>
                        <tr>
                          <td width="20"></td>
                          <td>
                            <span style="visibility:hidden;position:absolute" name="AssemblyElements">
                              <xsl:attribute name="id">
                                <xsl:value-of select="concat(substring($Category,11), '_', $source-id)"/>
                              </xsl:attribute>
                              <table width="100%" cellspacing="1" border="0" bgcolor="#efefef">
                                <xsl:if test="count(.//Message[@Category =$Category]/Issue[@Level = 'CriticalError']) > 0">
                                  <tr bgcolor="#efefef">
                                    <td colspan="6">
                                      <b>Critical Errors</b>
                                    </td>
                                  </tr>

                                  <xsl:for-each select=".//Message[@Category =$Category]/Issue[@Level = 'CriticalError']">
                                    <tr bgcolor="white">
                                      <td align="right" width="20" valign="Top">
                                        <font size="2" color="black">
                                          <xsl:number value="position()" format="1. " />
                                        </font>
                                      </td>
                                      <td>
                                        <xsl:value-of select="." />
                                        <xsl:if test="@File != ''">
                                          <br />
                                          File: <xsl:value-of select="@File" />  [Line(<xsl:value-of select="@Line" />)]
                                        </xsl:if>
                                        <br />
                                        <xsl:variable name="RuleID" select="../@CheckId" />
                                        <xsl:call-template name="FxCopRules">
                                          <xsl:with-param name="RuleID" select="$RuleID" />
                                        </xsl:call-template>
                                      </td>
                                    </tr>

                                  </xsl:for-each>
                                </xsl:if>
                                
                                <xsl:if test="count(.//Message[@Category =$Category]/Issue[@Level = 'Error']) > 0">
                                  <tr bgcolor="#efefef">
                                    <td colspan="6">
                                      <b>Errors</b>
                                    </td>
                                  </tr>

                                  <xsl:for-each select=".//Message[@Category =$Category]/Issue[@Level = 'Error']">
                                    <tr bgcolor="white">
                                      <td align="right" width="20" valign="Top">
                                        <font size="2" color="black">
                                          <xsl:number value="position()" format="1. " />
                                        </font>
                                      </td>
                                      <td>
                                        <xsl:value-of select="." />
                                        <xsl:if test="@File != ''">
                                          <br />
                                          File: <xsl:value-of select="@File" />  [Line(<xsl:value-of select="@Line" />)]
                                        </xsl:if>
                                        <br />
                                        <xsl:variable name="RuleID" select="../@CheckId" />
                                        <xsl:call-template name="FxCopRules">
                                          <xsl:with-param name="RuleID" select="$RuleID" />
                                        </xsl:call-template>
                                      </td>
                                    </tr>
                                  </xsl:for-each>
                                </xsl:if>
                                
                                <xsl:if test="count(.//Message[@Category =$Category]/Issue[@Level = 'CriticalWarning']) > 0">
                                  <tr bgcolor="#efefef">
                                  <td colspan="6">
                                    <b>Critical Warnings</b>
                                  </td>
                                </tr>

                                <xsl:for-each select=".//Message[@Category =$Category]/Issue[@Level = 'CriticalWarning']">
                                  <tr bgcolor="white">
                                    <td align="right" width="20" valign="Top">
                                      <font size="2" color="black">
                                        <xsl:number value="position()" format="1. " />
                                      </font>
                                    </td>
                                    <td>
                                      <xsl:value-of select="." />
                                      <xsl:if test="@File != ''">
                                        <br />
                                        File: <xsl:value-of select="@File" />  [Line(<xsl:value-of select="@Line" />)]
                                      </xsl:if>
                                      <br />
                                      <xsl:variable name="RuleID" select="../@CheckId" />
                                      <xsl:call-template name="FxCopRules">
                                        <xsl:with-param name="RuleID" select="$RuleID" />
                                      </xsl:call-template>
                                    </td>
                                  </tr>
                                </xsl:for-each>
                                </xsl:if>
                                
                                <xsl:if test="count(.//Message[@Category =$Category]/Issue[@Level = 'Warning']) > 0">
                                <tr bgcolor="#efefef">
                                  <td colspan="6">
                                    <b>Warnings</b>
                                  </td>
                                </tr>

                                <xsl:for-each select=".//Message[@Category =$Category]/Issue[@Level = 'Warning']">
                                  <tr bgcolor="white">
                                    <td align="right" width="20" valign="Top">
                                      <font size="2" color="black">
                                        <xsl:number value="position()" format="1. " />
                                      </font>
                                    </td>
                                    <td>
                                      <xsl:value-of select="." />
                                      <xsl:if test="@File != ''">
                                        <br />
                                        File: <xsl:value-of select="@File" />  [Line(<xsl:value-of select="@Line" />)]
                                      </xsl:if>
                                      <br />
                                      <xsl:variable name="RuleID" select="../@CheckId" />
                                      <xsl:call-template name="FxCopRules">
                                        <xsl:with-param name="RuleID" select="$RuleID" />
                                      </xsl:call-template>
                                    </td>
                                  </tr>

                                </xsl:for-each>
                                </xsl:if>
                              </table>
                            </span>
                          </td>
                        </tr> 
                      </table>
                    </td>
                  </tr>
                </table>
              </xsl:if>
            </xsl:for-each>
          </span>
        </td>
      </tr>
    </xsl:for-each>
 </xsl:template>
 
 <xsl:template match="/FxCopReport">

   <html>
     <head>
       <title>FxCop Analysis Report</title>
     </head>

     <style>
       H1
       {
       BACKGROUND-COLOR: #003366;
       BORDER-BOTTOM: #336699 6px solid;
       COLOR: #ffffff;
       FONT-SIZE: 130%;
       FONT-WEIGHT: normal;
       MARGIN: -14px;
       PADDING-BOTTOM: 8px;
       PADDING-LEFT: 30px;
       PADDING-TOP: 16px
       }
       H2
       {
       COLOR: #000000;
       FONT-SIZE: 100%;
       FONT-WEIGHT: bold;
       MARGIN-BOTTOM: 3px;
       MARGIN-LEFT: 10px;
       MARGIN-TOP: 20px;
       PADDING-LEFT: 0px
       }

       H3
       {
       COLOR: #000000;
       FONT-SIZE: 80%;
       FONT-WEIGHT: bold;
       MARGIN-BOTTOM: 3px;
       MARGIN-LEFT: 10px;
       MARGIN-TOP: 20px;
       PADDING-LEFT: 0px
       }

       #Title {font-family: Arial; font-size: 14pt; color: black; font-weight: bold}
       .ColumnHeader {font-family: Arial; font-size: 8pt; background-color:white; color: black}
       .CriticalError {font-family: Arial; font-size: 8pt; color: darkred; font-weight: bold; vertical-align: middle; }
       .Error {font-family: Arial; font-size: 8pt; color: royalblue; font-weight: bold; vertical-align: middle; }
       .CriticalWarning {font-family: Arial; font-size: 8pt; color: darkorange; font-weight: bold; vertical-align: middle; }
       .Warning {font-family: Arial; font-size: 8pt; color: darkgray; font-weight: bold; vertical-align: middle; }
       .Information {font-family: Arial; font-size: 8pt; color: black; font-weight: bold; vertical-align: middle; }

       .PropertyName {font-family: Arial; font-size: 8pt; color: black; font-weight: bold}
       .PropertyContent {font-family: Arial; font-size: 8pt; color: black}
       .NodeIcon { font-family: WebDings; font-size: 12pt; color: navy; padding-right: 5;}
       .MessagesIcon { font-family: WebDings; font-size: 12pt; color: red;}
       .RuleDetails { padding-top: 10;}
       .SourceCode { background-color:#DDDDFF; }
       .RuleBlock { background-color:#EEEEFF; }
       .MessageNumber { font-family: Arial; font-size: 10pt; color: darkred; }
       .MessageBlock { font-family: Arial; font-size: 10pt; color: darkred; }
       .Resolution {font-family: Arial; font-size: 8pt; color: black; }
       .NodeLine { font-family: Arial; font-size: 9pt;}
       .Note { font-family: Arial; font-size: 9pt; color:black; background-color: #DDDDFF; }
       .NoteUser { font-family: Arial; font-size: 9pt; font-weight: bold; }
       .NoteTime { font-family: Arial; font-size: 8pt; font-style: italic; }
       .Button { font-family: Arial; font-size: 9pt; color: blue; background-color: #EEEEEE; border-style: outset;}
       a:link { color: blue; text-decoration: none; }
       a:visited { color: blue; text-decoration: none; }
       a:active { color: blue; text-decoration: none; }
       TD
       {
       font-family:Arial;
       font-size:12px;
       }

       .f10b
       {
       font-family:Arial;
       font-size:10px;
       font-weight:bold;
       }

       .f11b
       {
       font-family:Arial;
       font-size:11px;
       font-weight:bold;
       }

       .f12b
       {
       font-family:Arial;
       font-size:12px;
       font-weight:bold;

       .f14b
       {
       font-family:Arial;
       font-size:14px;
       font-weight:bold;
       }

       .TDHeader
       {
       background-color:#cecf9c;
       font-family:Arial;
       font-size:11px;
       font-weight:bold;
       color:black;
       text-align:center;
       }
       .SecHeader
       {
       font-family:Arial;
       font-size:14px;
       font-weight:bold;
       margin:10;
       letter-spacing: 3px;
       background-repeat: no-repeat;
       text-indent:  35px;
       height:30;
       }

       .PageTitle
       {
       BACKGROUND-COLOR: #003366;
       BORDER-BOTTOM: #336699 6px solid;
       COLOR: #ffffff;
       FONT-SIZE: 130%;
       FONT-WEIGHT: normal;
       MARGIN: -14px;
       PADDING-LEFT: 10px;
       height:35;
       }

       .SectionTitle
       {
       BACKGROUND-COLOR: #003366;
       BORDER-BOTTOM: #336699 6px solid;
       COLOR: #ffffff;
       FONT-SIZE: 100%;
       FONT-WEIGHT: normal;
       MARGIN: -14px;
       PADDING-LEFT: 10px;
       height:20;
       }


       .ItemRow
       {
       writing-mode: tb-rl;
       filter: flipV flipH;
       font-family:arial;
       font-size:16px;
       font-weight:bold;
       color:white;
       background-color:#397FE8;
       letter-spacing: 3px;
       }

       body
       {
       top-margin:0;
       left-margin:0;
       right-margin:0;
       }
     </style>
     <script type="text/javascript">
       // <![CDATA[

       function ManageTreeMenu(iconID, divID, closeChildElements)
       {
           objDiv = document.getElementById(divID);
           objIcon = document.getElementById(iconID);
           if( null != objDiv.style)
           {
               if(objDiv.style.visibility == "hidden")
               {
                 objDiv.style.visibility = "visible";
                 objDiv.style.position = "";
                 objIcon.innerHTML = "^ ";
               }
           else
           {
                objDiv.style.visibility = "hidden";
                objDiv.style.position = "absolute";
                objIcon.innerHTML = "> ";
                // Hide All Child elements
                if(closeChildElements)      
                {
                    childElements =  document.getElementsByTagName("span");
                    var i ; 
                    for(i in childElements) 
                    {
                        elementID = childElements[i].id 
                        if(elementID != null)
                        {
                          if(elementID.indexOf(divID) != -1)
                          {
                            childElements[i].style.visibility = "hidden";
                            childElements[i].style.position = "absolute";
                            var temp = elementID + "_Marker";
                          
                            try
                            {
                              var objMarker =  objIcon = document.getElementById(temp);
                              objMarker.innerHTML = "> ";
                            }
                            catch(er){}
                          }
                        }
                    }
                }
            }
          }
       }
     // ]]>
</script>

     <body alink="Black" vlink="Black" link="Black" topmargin="0" leftmargin="0" rightmargin="0">
       <table style="width:100%" cellspacing="0" cellpadding="0">
         <tr nowrap="true">
           <td class="PageTitle" valign="center">
             FxCop - Static Code Analysis Report
           </td>
         </tr>
       </table>

       <xsl:for-each select="/FxCopReport">
         <table cellspacing="1" cellpadding="3" border="0" bgcolor="#efefef">
           <tr bgcolor="#efefef">
             <td width="5" bgcolor="#EFEFEF" rowspan="12"></td>
             <td align="center">
               <b>Category</b>
             </td>
             <td align="center">
               <b>Defects</b>
             </td>
             <td width="5" bgcolor="#EFEFEF" rowspan="12"></td>
             <td width="150">
               <b>Crit.Errors</b>
             </td>
             <td width="150">
               <b>Crit.Warnings</b>
             </td>
             <td width="150">
               <b>Errors</b>
             </td>
             <td width="150">
               <b>Warnings</b>
             </td>
             <td width="5" bgcolor="#EFEFEF" rowspan="12"></td>
           </tr>
           <xsl:for-each select=".//Message[generate-id()=generate-id(key('distinct-category',@Category))]">
             <xsl:call-template name="FxCopReport">
               <xsl:with-param name="Category" select="@Category" />
             </xsl:call-template>
           </xsl:for-each>
           <tr bgcolor="#efefef">

             <td height="5" colspan="2" bgcolor="white" wrap="false" align="right">
               Legend: <img src="App_Images/space.gif" />
               <img border="0" src="App_Images/bar_critical_error.gif" height="20" width="20" />
               <img src="App_Images/space.gif" />Critical Errors
               <img border="0" src="App_Images/bar_critical_warning.gif" height="20" width="20" />
               <img src="App_Images/space.gif" />Critical Warnings
               <img border="0" src="App_Images/bar_error.gif" height="20" width="20" />
               <img src="App_Images/space.gif" />Errors
               <img border="0" src="App_Images/bar_warning.gif" height="20" width="20" />
               <img src="App_Images/space.gif" />Warnings
               <img src="App_Images/space.gif" width="50"/>
               <b>Total</b>
             </td>

             <td align="right">
               <b>
                 <xsl:value-of select="count(.//Message[@Category !='']/Issue[@Level='CriticalError'])"/>
               </b>
             </td>
             <td align="right">
               <b>
                 <xsl:value-of select="count(.//Message[@Category !='']/Issue[@Level='CriticalWarning'])"/>
               </b>
             </td>
             <td align="right">
               <b>
                 <xsl:value-of select="count(.//Message[@Category !='']/Issue[@Level='Error'])"/>
               </b>
             </td>
             <td align="right">
               <b>
                 <xsl:value-of select="count(.//Message[@Category !='']/Issue[@Level='Warning'])"/>
               </b>
             </td>
           </tr>
           <tr>
             <td colspan="9" height="5" bgcolor="#efefef" align="center"></td>
           </tr>
         </table>
       </xsl:for-each>

       <br/>
       <table style="width:100%" cellspacing="0" cellpadding="0">
         <tr nowrap="true">
           <td class="SectionTitle" valign="center">
             Details
           </td>
         </tr>
       </table>
       <br/>
       <!-- FxCop Detailed Report -->
       <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
         <xsl:for-each select=".//Message[generate-id()=generate-id(key('distinct-category',@Category))]">
           <xsl:call-template name="DetailFxCopReport">
             <xsl:with-param name="Category" select="@Category" />
           </xsl:call-template>
         </xsl:for-each>
       </table>
     </body>
   </html>
   </xsl:template>      
   
</xsl:stylesheet>
