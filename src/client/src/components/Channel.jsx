import Toast from 'react-bootstrap/Toast';
import Badge from 'react-bootstrap/Badge';
import { useState } from 'react';


import EditSensor from './EditStack';
import Tips from '../components/Tips';

import { FaCheck, FaRegClock  } from "react-icons/fa";
import { IoMdClose, IoIosPause } from "react-icons/io";
import { BsExclamation } from "react-icons/bs";




function Channel({channel}) {

    const [showModalSensor, setShowModalSensor] = useState(false);
    
    const handleEditSensor = () => {
        console.log("Form enviado "+channel.id);
        setShowModalSensor(true);
    }

    let type = 'danger'; 
    switch (channel.current_metric.status) {
        case 'Success':
            type = 'success';
            break;

        case 'Warning':
            type = 'warning';
            break;
        
        case 'Paused':
            type = 'primary';
            break;

        case 'Error':
            type = 'danger';
            break;

        case 'NotChecked':
            type = 'secondary';
            break;

        default: type = 'danger';
            break;
    }


    return (
        <>
            <Toast style={{ marginLeft: '12px', marginRight: '-9px', width: '180px', height: '45px', display: 'flex', flexDirection: 'column', justifyContent: 'flex-start', paddingRight: '50px', position: 'relative' }}
                onDoubleClick={(e) => handleEditSensor()}
                >
                    <div className={"text-bg-"+type} style={{borderStyle: "none", position: "absolute", width: "14px", height: "104%", marginLeft: "-13px", marginTop: '-1px',  borderRadius: "3px", borderTopRightRadius: 0, borderEndEndRadius: 0}}>
                        {
                            (channel.current_metric.status == 'Error')
                            ?
                                <IoMdClose style={{height: '12px'}} />
                            :
                                <></>
                        }
                        {
                            (channel.current_metric.status == 'Success')
                            ?
                                <FaCheck style={{height: '9px'}} />
                            :
                                <></>
                        }
                        {
                            (channel.current_metric.status == 'Warning')
                            ?
                                <BsExclamation style={{height: '13px'}} />
                            :
                                <></>
                        }
                        {
                            (channel.current_metric.status == 'Paused')
                            ?
                                <IoIosPause style={{height: '12px'}} />
                            :
                                <></>
                        }
                        {
                            (channel.current_metric.status == 'NotChecked')
                            ?
                                <FaRegClock style={{height: '10px'}} />
                            :
                                <></>
                        }
                    </div>
                    <Tips message={(channel.current_metric.status == 'Error') ? channel.current_metric.message : channel.current_metric.status}>
                        <a style={{ fontSize: '10px', position: 'absolute', zIndex: 1, textAlign: 'left', marginTop: '5px', marginLeft: '9px' }}>
                            {channel.name} {channel.error} {/* {(title.length > 30) ? title.substr(0, 35) + ' ...' : title} */}
                        </a>
                        <Badge style={{ position: 'absolute', right: '5px', bottom: '5px', zIndex: 2 }} bg={type}>
                            {channel.current_metric.value} {channel.unit}
                        </Badge>
                    </Tips>
            </Toast>
        </>
        



    ); 
}

export default Channel;