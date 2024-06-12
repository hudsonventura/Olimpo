import Toast from 'react-bootstrap/Toast';
import Badge from 'react-bootstrap/Badge';
import { useState } from 'react';


import EditSensor from './EditStack';
import Tips from '../components/Tips';

import { FaCheck } from "react-icons/fa";
import { IoMdClose } from "react-icons/io";
import { BsExclamation } from "react-icons/bs";




function Channel({channel}) {

    const [showModalSensor, setShowModalSensor] = useState(false);
    
    const handleEditSensor = () => {
        console.log("Form enviado "+channel.id);
        setShowModalSensor(true);
    }

    let type = (channel.current_metric.error_code > 0) ? 'danger' : 'success';

    return (
        <>
            <Toast style={{ marginLeft: '12px', marginRight: '-9px', width: '180px', height: '45px', display: 'flex', flexDirection: 'column', justifyContent: 'flex-start', paddingRight: '50px', position: 'relative' }}
                onDoubleClick={(e) => handleEditSensor()}
                >
                    <div className={"text-bg-"+type} style={{borderStyle: "none", position: "absolute", width: "15px", height: "100%", marginLeft: "-12px", marginTop: 0,  borderRadius: "3px"}}>
                        {
                            (type == 'danger')
                            ?
                                <IoMdClose style={{height: '12px'}} />
                            :
                                <></>
                        }
                        {
                            (type == 'success')
                            ?
                                <FaCheck style={{height: '9px'}} />
                            :
                                <></>
                        }
                        {
                            (type == 'warning')
                            ?
                                <BsExclamation style={{height: '13px'}} />
                            :
                                <></>
                        }
                    </div>
                    <Tips message={(channel.current_metric.error_code > 0) ? channel.current_metric.message : 'Success'}>
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