﻿


declare namespace Scripter {

    namespace Module {

        namespace Common {

            interface GuidHelper {

                Parse(guid: string): System.Guid;
                New(): System.Guid;
                Empty(): System.Guid;

            }

            interface JsonConverter {

                Parse(json: string): any;
            }
        }
    }

}

declare namespace System {

    interface Guid {

    }
}

