import { useState } from 'react';

import "../components/Channel.css";

import Toast from 'react-bootstrap/Toast';
import Badge from 'react-bootstrap/Badge';


import Tips from '../components/Tips';
import EditChannel from '../components/EditChannel';


import { FaCheck, FaRegClock  } from "react-icons/fa";
import { IoMdClose, IoIosPause } from "react-icons/io";
import { BsExclamation, BsGearFill } from "react-icons/bs";
import { TbPlugX } from "react-icons/tb";





function Channel({channel, readOnlyMode}) {

    const [showModalEditChannel, setShowModalEditChannel] = useState(false);

    

    let type = 'danger'; 
    let bg_type = 'text-bg-danger';
    let line = 'red';
    switch (channel.current_metric.status) {
        case 'Success':
            type = 'success';
            bg_type = 'text-bg-success';
            line = '#22841c';
            break;

        case 'Warning':
            type = 'warning';
            bg_type = 'text-bg-warning';
            line = '#ffc107';
            break;
        
        case 'Paused':
            type = 'primary';
            bg_type = 'text-bg-primary';
            line = '#0d6efd';
            break;

        case 'Error':
            type = 'danger';
            bg_type = 'text-bg-danger';
            line = '#dc3545';
            break;

        case 'NotChecked':
            type = 'secondary';
            bg_type = 'text-bg-secondary';
            line = '#6c757d';
            break;

        case 'Offline':
            type = 'offline';
            bg_type = 'offline';
            line = '#992222';
            break;

    }



    const handleEditChannel = ({channel}) => {
        setShowModalEditChannel(true);
    }




    return (
        <>
            <EditChannel showModal={showModalEditChannel} setShowModal={setShowModalEditChannel} channel={channel}/>
            <Toast style={{ marginLeft: '12px', marginRight: '-9px', width: '180px', height: '45px', display: 'flex', flexDirection: 'column', justifyContent: 'flex-start', paddingRight: '50px', position: 'relative', borderStyle: 'solid', borderColor: line }}
                >
                    <div className={bg_type} style={{borderStyle: "none", position: "absolute", width: "14px", height: "104%", marginLeft: "-13px", marginTop: '-1px',  borderRadius: "3px", borderTopRightRadius: 0, borderEndEndRadius: 0}}>
                        {
                            (channel.current_metric.status == 'Error')
                            ?
                                <IoMdClose style={{height: '12px'}} />
                            :
                                <></>
                        }
                        {
                            (channel.current_metric.status == 'Offline')
                            ?
                                <TbPlugX style={{height: '16px'}} />
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
                        {
                            (!readOnlyMode)
                            ? <BsGearFill style={{height: '10px', rotate: 'true' }} animation="border"   onClick={() => handleEditChannel(channel)}/>
                            : <> </>
                        }
                    </div>
                    <Tips message={(channel.current_metric.status == 'Error') ? channel.current_metric.message : channel.current_metric.status}>
                        <a style={{ fontSize: '10px', position: 'absolute', zIndex: 1, textAlign: 'left', marginTop: '5px', marginLeft: '9px', width: "140px" }}>
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