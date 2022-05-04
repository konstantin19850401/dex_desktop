<?php
    if (lib_session_ping($sid)) {
        if ($dblink) {
            $_db = $config_db[$db];
            if (strstr($_db["engine"], "efd")) {
                // Справочники ЕФД
                $diclist = array(
                    "efd_client_cats" => "1",
                    "efd_client_types" => "1",
                    "efd_registr_cats" => "1",
                    "efd_client_resident_types" => "1",
                    "efd_genders" => "1",
                    "efd_jur_types" => "1",
                    "efd_identity_types" => "1",
                    "efd_delivery_types" => "1",
                    "efd_country" => "1"
                );
            }

			if (strstr($_db["engine"], "beeline")) {
				// Справочники Билайн
				$diclist = array(
					"beeline_billcycles" => "1",
					"beeline_billplans" => "1",
					"beeline_bplanservices" => "1",
					"beeline_buildingtypes" => "1",
					"beeline_cellnets" => "1",
					"beeline_channellens" => "1",
					"beeline_channeltypes" => "1",
					"beeline_companytypes" => "1",
					"beeline_dealeroffices" => "1",
					"beeline_deliverytypes" => "1",
					"beeline_doctypes" => "1",
					"beeline_logparams" => "1",
					"beeline_offices" => "1",
					"beeline_paysystems" => "1",
					"beeline_persontypes" => "1",
					"beeline_placetypes" => "1",
					"beeline_roomtypes" => "1",
					"beeline_services" => "1",
					"beeline_spheres" => "1",
					"beeline_streettypes" => "1"
				);
			}

			if (strstr($_db["engine"], "mts")) {
				// Справочники МТС
				$diclist = array(
					"mts_doccountry" => "1",
					"mts_doctype" => "1"
				);
			}
			
            // Общие для всех конфигураций справочники
            $diclist["registers"] = "1";
            $diclist["city"] = "1";
            $diclist["um_plans"] = "1";
            $diclist["um_regions"] = "1";

            if($result = mysql_query("show table status", $dblink)) {
                while($row = mysql_fetch_assoc($result)) {
                    if ($diclist[$row["Name"]]) {
                        $diclist[$row["Name"]] = md5($row["Update_time"]);
                    }
                }
            }

            $_reply = $RESULT_OK;
            $_reply["data"] = $diclist;
        }
    } else {
        $_reply = $RESULT_NEED_AUTH;
    }

?>
